using AutoMapper;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class SessionMapper : Profile
    {
        public SessionMapper()
        {
            // Session -> CreateSessionRequestDTO
            CreateMap<Session, CreateSessionRequestDTO>();

            // CreateSessionRequestDTO -> Session
            CreateMap<CreateSessionRequestDTO, Session>();
        }
    }
}