using AutoMapper;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.Course;
using ebyteLearner.Helpers;
using ebyteLearner.Models;

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
        private readonly ILogger<CourseService> _logger;
        private readonly IMapper _mapper;
        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CourseDTO> GetCourse(Guid id)
        {
            return await _courseRepository.Read(id);
        }

        public async Task CreateCourse(CreateCourseRequestDTO request)
        {
            await _courseRepository.Create(request);
        }

        public async Task<CourseDTO> UpdateCourse(Guid id, UpdateCourseRequestDTO request)
        {
            return await _courseRepository.Update(id,request);
        }

        public async Task DeleteCourse(Guid id)
        {
            await _courseRepository.Delete(id);
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCourses()
        {
            return await _courseRepository.ReadAllCourses();
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
