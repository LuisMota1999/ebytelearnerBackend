using AutoMapper;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class ModuleMapper : Profile
    {
        public ModuleMapper()
        {

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
        }
    }
}