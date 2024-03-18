using AutoMapper;
using ebyteLearner.DTOs.Category;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CreateCategoryRequestDTO, Category>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CreateCategoryRequestDTO, Category>().ReverseMap();
            CreateMap<Category, UpdateCategoryRequestDTO>().ReverseMap();
            CreateMap<UpdateCategoryRequestDTO, CategoryDTO>().ReverseMap();
        }
    }
}
