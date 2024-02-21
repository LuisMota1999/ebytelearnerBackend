using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using ebyteLearner.DTOs.User;
using ebyteLearner.Helpers;
using ebyteLearner.Models;

namespace ebyteLearner.Services
{
    public interface IModuleService
    {
        Task CreateModule(CreateModuleRequestDTO request);
        Task<ModuleDTO> GetModule(Guid id);
        Task<ModuleDTO> UpdateModule(Guid Id, UpdateModuleRequestDTO request);
        Task DeleteModule(Guid id);

    }

    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleService(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task CreateModule(CreateModuleRequestDTO request)
        {
            await _moduleRepository.Create(request);
        }
        public async Task<ModuleDTO> UpdateModule(Guid Id, UpdateModuleRequestDTO request)
        {

            var updatedUser = await _moduleRepository.Update(Id, request);
            return updatedUser;
        }

        public async Task<ModuleDTO> GetModule(Guid id)
        {
            return await _moduleRepository.Read(id);
        }
        
        public async Task DeleteModule(Guid id)
        {
            await _moduleRepository.Delete(id);
        }
    }
}
