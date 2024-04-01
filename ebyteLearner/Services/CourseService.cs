using AutoMapper;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.Course;
using ebyteLearner.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllCourses(int returnRows = 0);
        Task<CourseDTO> GetCourse(Guid id);
        Task<int> CreateCourse(CreateCourseRequestDTO request);
        Task<(int rows, CourseDTO course)> UpdateCourse(Guid id, UpdateCourseRequestDTO request);
        Task DeleteCourse(Guid id);
        Task<int> AssocModuleToCourse(AssociateModuleRequest associateModuleRequest);
        Task<(int rows, CourseDTO course)> UploadCourseImage(MemoryStream file, Guid courseId, string fileName, string contentType);
    }

    public class CourseService : ICourseService
    {

        private readonly ICourseRepository _courseRepository;
        private readonly ICacheService _cacheService;
        private readonly IDriveServiceHelper _driveServiceHelper;
        private readonly ILogger<CourseService> _logger;
        private readonly IMapper _mapper;
        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger, IMapper mapper, ICacheService cacheService, IDriveServiceHelper driveServiceHelper)
        {
            _courseRepository = courseRepository;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _driveServiceHelper = driveServiceHelper ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public async Task<CourseDTO> GetCourse(Guid id)
        {
            //var cachedCourse = _cacheService.GetData<CourseDTO>(id.ToString());
            //if (cachedCourse != null)
            //    return cachedCourse;

            //var expiryTime = DateTimeOffset.Now.AddMinutes(60);

            var response = await _courseRepository.Read(id);
            //_cacheService.SetData<CourseDTO>(id.ToString(), response, expiryTime);

            return response;
        }

        public async Task<(int rows, CourseDTO course)> UploadCourseImage(MemoryStream file, Guid courseId, string fileName, string contentType)
        {

            try
            {
                var cachedCourses = _cacheService.GetData<IEnumerable<CourseDTO>>("GetAllCourses");
                if (cachedCourses != null)
                    _cacheService.RemoveData("GetAllCourses");

                if (file.Length <= 0)
                {
                    _logger.LogError("Uploading file to Google Drive failed. Can not upload 0 byte file to drive.");
                    throw new ValidationException();
                }

                var course = await _courseRepository.Read(courseId);
                if (course == null)
                {
                    _logger.LogError($"Course {courseId} not found");
                    throw new ValidationException($"Course {courseId} not found");
                }

                var description = "File_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

                var imagepath = await _driveServiceHelper.UploadFile(file, fileName, contentType, course.CourseDirectory, description);

                _logger.LogInformation("Uploading file to Google Drive complete. Image Url: ", imagepath);

                if (imagepath.IsNullOrEmpty())
                {
                    throw new ValidationException($"Image Url not found");
                }

                var updateRequest = new UpdateCourseRequestDTO();
                updateRequest.CourseImageURL = imagepath;
                var (rows, response) = await _courseRepository.Update(courseId, updateRequest);
                return (rows, response);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<int> CreateCourse(CreateCourseRequestDTO request)
        {
            var cachedCourses = _cacheService.GetData<IEnumerable<CourseDTO>>("GetAllCourses");
            if (cachedCourses != null)
                _cacheService.RemoveData("GetAllCourses");

            request.CourseDirectory = await _driveServiceHelper.CreateFolder(request.CourseName);
            return await _courseRepository.Create(request);
        }

        public async Task<(int, CourseDTO)> UpdateCourse(Guid id, UpdateCourseRequestDTO request)
        {
            var cachedCourses = _cacheService.GetData<IEnumerable<CourseDTO>>("GetAllCourses");
            if (cachedCourses != null)
                _cacheService.RemoveData("GetAllCourses");
            var (rows, response) = await _courseRepository.Update(id, request);
            return (rows, response);
        }

        public async Task DeleteCourse(Guid id)
        {
            _cacheService.RemoveData(id.ToString());

            await _courseRepository.Delete(id);
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCourses(int returnRows = 10)
        {
            var cachedCourses = _cacheService.GetData<IEnumerable<CourseDTO>>("GetAllCourses");
            if (cachedCourses != null)
                return cachedCourses.TakeLast(returnRows);

            var expiryTime = DateTimeOffset.Now.AddMinutes(5);

            var response = await _courseRepository.ReadAllCourses();

            _cacheService.SetData<IEnumerable<CourseDTO>>("GetAllCourses", response, expiryTime);

            if (returnRows > 0)
            {
                response = response.TakeLast(returnRows);
            }

            return response;
        }

        public async Task<int> AssocModuleToCourse(AssociateModuleRequest associateModuleRequest)
        {
            return await _courseRepository.AssociateModuleToCourse(associateModuleRequest.CourseID, associateModuleRequest.ModuleID);
        }

    }


}
