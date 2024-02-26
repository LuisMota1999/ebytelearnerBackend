using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using ebyteLearner.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ebyteLearner.Data.Repository
{
    public interface ICourseRepository
    {
        Task<CourseDTO> Update(Guid id, UpdateCourseRequestDTO request);
        Task<CourseDTO> Read(Guid id);
        Task<IEnumerable<CourseDTO>> ReadAllCourses();
        Task Delete(Guid id);
        Task Create(CreateCourseRequestDTO request);
        Task AssociateModuleToCourse(Guid courseId, Guid moduleId);
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

        public async Task<CourseDTO> Read(Guid id)
        {
            var courseWithModules = await _dbContext.Course
                .Where(course => course.Id == id)
                .Select(course => new CourseDTO
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                    Modules = course.Modules.Select(module => new ModuleDTO
                    {
                        Id = module.Id,
                        ModuleName = module.ModuleName
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (courseWithModules == null)
            {
                throw new AppException($"Course '{id}' not found");
            }

            return courseWithModules;
        }

        public async Task Create(CreateCourseRequestDTO request)
        {
            if (_dbContext.Course.Any(x => x.CourseName == request.CourseName))
                throw new AppException("Course '" + request.CourseName + "' is already registered");

            var course = _mapper.Map<Course>(request);

            _dbContext.Course.Add(course);

            await _dbContext.SaveChangesAsync();
        }
        public async Task<CourseDTO> Update(Guid id, UpdateCourseRequestDTO request)
        {
            var courseDB = _dbContext.Course.Find(id);
            if (courseDB != null)
            {
                _mapper.Map(request, courseDB);
                await _dbContext.SaveChangesAsync();
                var updatedCourseResponse = _mapper.Map<CourseDTO>(courseDB);
                return updatedCourseResponse;
            }
            else
                throw new AppException("Course '" + id + "' not found");
        }
        public async Task Delete(Guid id)
        {
            var course = await _dbContext.Course.FindAsync(id);
            if (course != null)
            {
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

            if (coursesWithModules == null || !coursesWithModules.Any())
            {
                return null;
            }

            var courseDTOs = coursesWithModules.Select(course => new CourseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                Modules = course.Modules.Select(module => new ModuleDTO
                {
                    Id = module.Id,
                    ModuleName = module.ModuleName,
                }).ToList()
            }).ToList();

            return courseDTOs;
        }

        public async Task AssociateModuleToCourse(Guid courseId, Guid moduleId)
        {
            var module = new Module
            {
                Id = moduleId,
                CourseId = courseId
            };

            _dbContext.Attach(module);
            _dbContext.Entry(module).Property(m => m.CourseId).IsModified = true;

            await _dbContext.SaveChangesAsync();
        }
    }
}