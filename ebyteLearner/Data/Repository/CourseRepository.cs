using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using Microsoft.IdentityModel.Tokens;


namespace ebyteLearner.Data.Repository
{
    public interface ICourseRepository
    {
        Task<CourseDTO> Update(Guid id, UpdateCourseRequestDTO request);
        Task<CourseDTO> Read(Guid id);
        Task<IEnumerable<CourseDTO>> ReadAllCourses();
        Task Delete(Guid id);
        Task<int> Create(CreateCourseRequestDTO request);
        Task<int> AssociateModuleToCourse(Guid courseId, Guid moduleId);
    }

    public class CourseRepository : ICourseRepository
    {
        private readonly DBContextService _dbContext;
        private readonly ILogger<CourseRepository> _logger;
        private readonly IMapper _mapper;
        public CourseRepository(DBContextService dbContext, ILogger<CourseRepository> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Create(CreateCourseRequestDTO request)
        {
           
            if (_dbContext.Category.Find(request.CategoryId) == null)
                throw new AppException("Category '" + request.CategoryId + "' not found");
            
            if (_dbContext.User.Find(request.CourseTeacherID) == null)
                throw new AppException("Teacher '" + request.CategoryId + "' not found");

            if (request.CourseName.IsNullOrEmpty())
                throw new AppException($"Course name can not be empty");

            if (_dbContext.Course.Any(x => x.CourseName.Equals(request.CourseName)))
                throw new AppException("Course '" + request.CourseName + "' is already registered");

            var course = _mapper.Map<Course>(request);

            _dbContext.Course.Add(course);

            try
            {
                // Save changes asynchronously
                var rowsAffected = await _dbContext.SaveChangesAsync();

                // Log successful creation
                _logger.LogInformation($"Created course with ID: {course.Id}, rows affected: {rowsAffected}");

                return rowsAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating course");

                throw;
            }
        }

        public async Task<CourseDTO> Read(Guid id)
        {
            var courseWithModules = await _dbContext.Course
                .Where(course => course.Id == id)
                .Select(course => new CourseDTO
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    CourseModules = course.Modules.Select(module => new ModuleDTO
                    {
                        Id = module.Id,
                        ModuleName = module.ModuleName
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (courseWithModules == null)
            {
                _logger.LogWarning($"Course with ID: {id} not found!");
                throw new AppException($"Course '{id}' not found");
            }

            return courseWithModules;
        }

        public async Task<CourseDTO> Update(Guid id, UpdateCourseRequestDTO request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var courseDB = await _dbContext.Course.FindAsync(id);
            if (courseDB != null)
            {
                _mapper.Map(request, courseDB);
                _dbContext.Entry(courseDB).State = EntityState.Modified;

                try
                {
                    await _dbContext.SaveChangesAsync();
                    return _mapper.Map<CourseDTO>(courseDB);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle concurrency conflicts
                    // Log the exception and return an appropriate response
                    throw new AppException("Concurrency conflict occurred while updating the course.", ex);
                }
                catch (DbUpdateException ex)
                {
                    // Handle database update exception
                    // Log the exception and return an appropriate response
                    throw new AppException("Error occurred while updating the course in the database.", ex);
                }
            }
            else
            {
                throw new AppException($"Course with ID '{id}' not found.");
            }
        }

        public async Task Delete(Guid id)
        {
            var course = await _dbContext.Course.FindAsync(id);
            if (course != null)
            {
                _dbContext.Entry(course).State = EntityState.Deleted;
                _dbContext.Remove(course);
                await _dbContext.SaveChangesAsync();
            }
            else
                throw new AppException("Course '" + id + "' not found");
        }
        public async Task<IEnumerable<CourseDTO>> ReadAllCourses()
        {
            var coursesWithModules = await _dbContext.Course
                .Include(course => course.Modules)
                .ToListAsync();

            if (!coursesWithModules.Any())
            {
                return Enumerable.Empty<CourseDTO>();
            }

            var courseDTOs = coursesWithModules.Select(course => new CourseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                CoursePrice = course.CoursePrice,
                CourseModules = course.Modules.Select(module => new ModuleDTO
                {
                    Id = module.Id,
                    ModuleName = module.ModuleName,
                }).ToList()
            }).ToList();

            return courseDTOs;
        }

        public async Task<int> AssociateModuleToCourse(Guid courseId, Guid moduleId)
        {
            var course = await _dbContext.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
            {
                throw new AppException($"Course '{courseId}' not found");
            }

            var module = await _dbContext.Module.FindAsync(moduleId);
            if (module == null)
            {
                throw new AppException($"Module '{moduleId}' not found");
            }

            // Check if the module is already associated with the course
            if (course.Modules.Any(m => m.Id == moduleId))
            {
                throw new AppException($"Module '{moduleId}' is already associated with the course '{courseId}'");
            }

            // Associate the module with the course
            course.Modules.Add(module);

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception and return an appropriate response
                throw new AppException("Error occurred while associating the module with the course in the database.", ex);
            }
        }

    }
}