using AutoMapper;
using ebyteLearner.DTOs.Auth;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class AuthMapper : Profile
    {
        public AuthMapper()
        {
            CreateMap<UserDTO, AuthResponseDTO>();
            CreateMap<RegisterRequestDTO, User>();
        }
    }
}