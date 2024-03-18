using AutoMapper;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.Category;

namespace ebyteLearner.Services
{

    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategories(int returnRows = 10);
        Task<CategoryDTO> GetCategory(Guid id);
        Task CreateCategory(CreateCategoryRequestDTO request);
        Task<CategoryDTO> UpdateCategory(Guid id, UpdateCategoryRequestDTO request);
        Task DeleteCategory(Guid id);
    }

    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger, IMapper mapper, ICacheService cacheService)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService)); ;
        }

        public async Task<CategoryDTO> GetCategory(Guid id)
        {
            var cachedCourse = _cacheService.GetData<CategoryDTO>(id.ToString());
            if (cachedCourse != null)
                return cachedCourse;

            var expiryTime = DateTimeOffset.Now.AddMinutes(60);

            var response = await _categoryRepository.Read(id);
            _cacheService.SetData<CategoryDTO>(id.ToString(), response, expiryTime);

            return response;
        }

        public async Task CreateCategory(CreateCategoryRequestDTO request)
        {
            _cacheService.RemoveData("GetAllCategories");
            await _categoryRepository.Create(request);
        }

        public async Task<CategoryDTO> UpdateCategory(Guid id, UpdateCategoryRequestDTO request)
        {
            var response = await _categoryRepository.Update(id, request);
            var expiryTime = DateTimeOffset.Now.AddMinutes(60);
            _cacheService.SetData<CategoryDTO>(id.ToString(), response, expiryTime);
            return response;
        }

        public async Task DeleteCategory(Guid id)
        {
            _cacheService.RemoveData(id.ToString());

            await _categoryRepository.Delete(id);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories(int returnRows = 10)
        {
            var cachedCourses = _cacheService.GetData<IEnumerable<CategoryDTO>>("GetAllCategories");
            if (cachedCourses != null)
                return cachedCourses.TakeLast(returnRows);

            var expiryTime = DateTimeOffset.Now.AddMinutes(5);

            var response = await _categoryRepository.ReadAllCategories();

            _cacheService.SetData<IEnumerable<CategoryDTO>>("GetAllCourses", response, expiryTime);

            if (returnRows > 0)
            {
                response = response.TakeLast(returnRows);
            }

            return response;
        }
    }
}

