using AutoMapper;
using ebyteLearner.DTOs.Category;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, UpdateCategoryRequestDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
            .ReverseMap();

            CreateMap<Category, CreateCategoryRequestDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
            .ReverseMap();

            CreateMap<Category, CategoryDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
            .ReverseMap();
        }
    }
}
