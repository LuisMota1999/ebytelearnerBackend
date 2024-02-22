using AutoMapper;
using ebyteLearner.DTOs.Auth;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using ebyteLearner.DTOs.PDF;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // User -> UserDTO
            CreateMap<User, UserDTO>();

            // UserDTO -> User
            CreateMap<UserDTO, User>();
        }
    }
}