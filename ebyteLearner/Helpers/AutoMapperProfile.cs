using AutoMapper;
using ebyteLearner.DTOs.Auth;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using ebyteLearner.DTOs.NewFolder;
using ebyteLearner.DTOs.PDF;
using ebyteLearner.Models;

namespace ebyteLearner.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            // User -> AuthenticateResponse
            CreateMap<UserDTO, AuthResponseDTO>();

            // RegisterRequest -> User
            CreateMap<RegisterRequestDTO, User>();

            // User -> UserDTO
            CreateMap<User, UserDTO>();

            // UserDTO -> User
            CreateMap<UserDTO, User>();

            // CreateCourseDTO -> Course
            CreateMap<CreateCourseRequestDTO, Course>();

            // Course -> CourseDTO
            CreateMap<Course, CourseDTO>();

            // UpdateCourseRequest -> Course
            CreateMap<UpdateCourseRequestDTO, Course>();

            // UpdateCourseRequest -> CourseDTO
            CreateMap<UpdateCourseRequestDTO, CourseDTO>();

            // CreateModuleRequestDTO -> Module
            CreateMap<CreateModuleRequestDTO, Module>();

            // Module -> ModuleDTO
            CreateMap<Module, ModuleDTO>();

            // UpdateModuleRequestDTO -> Module
            CreateMap<UpdateModuleRequestDTO, Module>();

            // UpdateModuleRequestDTO -> ModuleDTO
            CreateMap<UpdateModuleRequestDTO, ModuleDTO>();

            // AssociateModuleRequest -> Module
            CreateMap<AssociateModuleRequest, Module>();

            // AssociateModuleRequest -> Module
            CreateMap<Module, AssociateModuleRequest>();

            // AssociateModuleRequest -> Module
            CreateMap<Module, AssociateModuleRequest>();

            // Pdf -> CreatePDFRequestDTO
            CreateMap<Pdf, CreatePDFRequestDTO>();

            // CreatePDFRequestDTO -> Pdf
            CreateMap<CreatePDFRequestDTO, Pdf>();

            // PDFDTO -> Pdf
            CreateMap<PDFDTO, Pdf>();

            // Pdf -> PDFDTO
            CreateMap<Pdf, PDFDTO>();

            // Session -> CreateSessionRequestDTO
            CreateMap<Session, CreateSessionRequestDTO>();

            // CreateSessionRequestDTO -> Session
            CreateMap<CreateSessionRequestDTO, Session>();

        }
    }
}
