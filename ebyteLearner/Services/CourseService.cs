using AutoMapper;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.Course;
using ebyteLearner.Helpers;

namespace ebyteLearner.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllCourses();
        Task<CourseDTO> GetCourse(Guid id);
        Task CreateCourse(CreateCourseRequestDTO request);
        Task<CourseDTO> UpdateCourse(Guid id, UpdateCourseRequestDTO request);
        Task DeleteCourse(Guid id);
        Task AssocModuleToCourse(AssociateModuleRequest associateModuleRequest);
    }

    public class CourseService : ICourseService
    {

        private readonly ICourseRepository _courseRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CourseService> _logger;
        private readonly IMapper _mapper;
        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger, IMapper mapper, ICacheService cacheService)
        {
            _courseRepository = courseRepository;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService)); ;
        }

        public async Task<CourseDTO> GetCourse(Guid id)
        {
            var cachedCourse = _cacheService.GetData<CourseDTO>(id.ToString());
            if (cachedCourse != null)
                return cachedCourse;

            var expiryTime = DateTimeOffset.Now.AddMinutes(60);

            var response = await _courseRepository.Read(id);
            _cacheService.SetData<CourseDTO>(id.ToString(), response, expiryTime);

            return response;
        }

        public async Task CreateCourse(CreateCourseRequestDTO request)
        {
            _cacheService.RemoveData("GetAllCourses");
            await _courseRepository.Create(request);
        }

        public async Task<CourseDTO> UpdateCourse(Guid id, UpdateCourseRequestDTO request)
        {
            var response = await _courseRepository.Update(id, request);
            var expiryTime = DateTimeOffset.Now.AddMinutes(60);
            _cacheService.SetData<CourseDTO>(id.ToString(), response, expiryTime);
            return response;
        }

        public async Task DeleteCourse(Guid id)
        {
            _cacheService.RemoveData(id.ToString());

            await _courseRepository.Delete(id);
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCourses()
        {
            var cachedCourses = _cacheService.GetData<IEnumerable<CourseDTO>>("GetAllCourses");
            if (cachedCourses != null)
                return cachedCourses;

            var expiryTime = DateTimeOffset.Now.AddMinutes(5);

            var response = await _courseRepository.ReadAllCourses();
            _cacheService.SetData<IEnumerable<CourseDTO>>("GetAllCourses", response, expiryTime);

            return response;
        }

        public async Task AssocModuleToCourse(AssociateModuleRequest associateModuleRequest)
        {
            var course = await _courseRepository.Read(associateModuleRequest.CourseID);

            if (course == null)
            {
                throw new AppException($"Course '{associateModuleRequest.CourseID}' not found");
            }

            if (course.Modules.Any(m => m.Id == associateModuleRequest.ModuleID))
            {
                throw new AppException($"Module '{associateModuleRequest.ModuleID}' is already associated with the course");
            }

            await _courseRepository.AssociateModuleToCourse(associateModuleRequest.CourseID, associateModuleRequest.ModuleID);
        }

    }


}
