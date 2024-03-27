using AutoMapper;
using ebyteLearner.DTOs.Course;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class CourseMapper : Profile
    {
        public CourseMapper()
        {
            CreateMap<UpdateCourseRequestDTO, Course>()
                .ForAllMembers(opt => opt.Condition((src, dest, prop) => prop != null));

            CreateMap<Course, UpdateCourseRequestDTO>()
                .ForAllMembers(opt => opt.Condition((src, dest, prop) => prop != null));

            // CreateMap para os outros mapeamentos
            CreateMap<CreateCourseRequestDTO, Course>().ReverseMap();
            CreateMap<Course, CourseDTO>().ReverseMap();
        }
    }
}