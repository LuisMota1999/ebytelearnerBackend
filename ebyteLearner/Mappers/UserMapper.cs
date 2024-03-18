using AutoMapper;
using ebyteLearner.DTOs.Auth;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using ebyteLearner.DTOs.PDF;
using ebyteLearner.DTOs.User;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UpdateUserRequestDTO>().ReverseMap();
            CreateMap<UserDTO, UpdateUserRequestDTO>().ReverseMap();
        }
    }
}