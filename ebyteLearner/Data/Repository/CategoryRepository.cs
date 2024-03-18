using AutoMapper;
using ebyteLearner.DTOs.Category;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using Microsoft.EntityFrameworkCore;

namespace ebyteLearner.Data.Repository
{
    public interface ICategoryRepository
    {
        Task Create(CreateCategoryRequestDTO request);
        Task Delete(Guid id);
        Task<CategoryDTO> Update(Guid id, UpdateCategoryRequestDTO request);
        Task<CategoryDTO> Read(Guid id);
        Task<IEnumerable<CategoryDTO>> ReadAllCategories();
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DBContextService _dbContext;
        private readonly ILogger<CategoryRepository> _logger;
        private readonly IMapper _mapper;
        public CategoryRepository(DBContextService dbContext, ILogger<CategoryRepository> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CategoryDTO> Read(Guid id)
        {
            var category = await _dbContext.Category
                .Where(category => category.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new AppException($"Category '{id}' not found");
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }

        public async Task Create(CreateCategoryRequestDTO request)
        {
            if (_dbContext.Category.Any(x => x.CategoryName.Equals(request.CategoryName)))
                throw new AppException("Category '" + request.CategoryName + "' is already registered");

            var category = _mapper.Map<Category>(request);

            _dbContext.Category.Add(category);

            await _dbContext.SaveChangesAsync();
        }
        public async Task<CategoryDTO> Update(Guid id, UpdateCategoryRequestDTO request)
        {
            var categoryDB = await _dbContext.Category.FindAsync(id);
            if (categoryDB != null)
            {
                _mapper.Map(request, categoryDB);
                _dbContext.Entry(categoryDB).State = EntityState.Modified;

                try
                {
                    await _dbContext.SaveChangesAsync();
                    return _mapper.Map<CategoryDTO>(categoryDB);
                }
                catch (DbUpdateConcurrencyException ex)
                {         
                    throw new AppException("Concurrency conflict occurred while updating the category.", ex);
                }
                catch (DbUpdateException ex)
                {
                    throw new AppException("Error occurred while updating the category in the database.", ex);
                }
            }
            else
            {
                throw new AppException($"Category with ID '{id}' not found.");
            }
        }

        public async Task Delete(Guid id)
        {
            var categoryDB = await _dbContext.Category.FindAsync(id);
            if (categoryDB != null)
            {
                _dbContext.Remove(categoryDB);
                await _dbContext.SaveChangesAsync();
            }
            else
                throw new AppException("Category '" + id + "' not found");
        }
        public async Task<IEnumerable<CategoryDTO>> ReadAllCategories()
        {
            var categories = await _dbContext.Category.ToListAsync();

            if (!categories.Any())
            {
                return Enumerable.Empty<CategoryDTO>();
            }

            var categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

            return categoryDTOs;
        }
    }
}
