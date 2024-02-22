using AutoMapper;
using ebyteLearner.DTOs.Course;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class CourseMapper : Profile
    {
        public CourseMapper()
        {
            // CreateCourseDTO -> Course
            CreateMap<CreateCourseRequestDTO, Course>();

            // Course -> CourseDTO
            CreateMap<Course, CourseDTO>();

            // UpdateCourseRequest -> Course
            CreateMap<Course, UpdateCourseRequestDTO>();

            // UpdateCourseRequest -> CourseDTO
            CreateMap<UpdateCourseRequestDTO, CourseDTO>();
        }
    }
}