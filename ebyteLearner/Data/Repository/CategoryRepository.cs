using AutoMapper;
using ebyteLearner.DTOs.Category;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Data.Repository
{
    public interface ICategoryRepository
    {
        Task<int> Create(CreateCategoryRequestDTO request);
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

        public async Task<int> Create(CreateCategoryRequestDTO request)
        {
            if (request.CategoryName.IsNullOrEmpty())
                throw new AppException($"Category name can not be empty");

            if (_dbContext.Category.Any(x => x.CategoryName.Equals(request.CategoryName)))
                throw new AppException("Category '" + request.CategoryName + "' is already registered");

            // Map DTO to entity
            var category = _mapper.Map<Category>(request);

            // Add category to DbContext
            _dbContext.Category.Add(category);

            try
            {
                // Save changes asynchronously
                var rowsAffected = await _dbContext.SaveChangesAsync();

                // Log successful creation
                _logger.LogInformation($"Created category with ID: {category.Id}, rows affected: {rowsAffected}");

                return rowsAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category");

                throw;
            }
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
