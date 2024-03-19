using AutoMapper;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.Module;

namespace ebyteLearner.Services
{
    public interface IModuleService
    {
        Task<int> CreateModule(CreateModuleRequestDTO request);
        Task<ModuleDTO> GetModule(Guid id);
        Task<ModuleDTO> UpdateModule(Guid Id, UpdateModuleRequestDTO request);
        Task DeleteModule(Guid id);

    }

    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ModuleService> _logger;
        private readonly IMapper _mapper;

        public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> logger, IMapper mapper, ICacheService cacheService)
        {
            _moduleRepository = moduleRepository;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService)); ;
        }

        public async Task<int> CreateModule(CreateModuleRequestDTO request)
        {
            return await _moduleRepository.Create(request);
        }
        public async Task<ModuleDTO> UpdateModule(Guid Id, UpdateModuleRequestDTO request)
        {
            var response = await _moduleRepository.Update(Id, request);
            var expiryTime = DateTimeOffset.Now.AddMinutes(60);
            _cacheService.SetData<ModuleDTO>(Id.ToString(), response, expiryTime);
            return response;
        }

        public async Task<ModuleDTO> GetModule(Guid id)
        {
            var cachedModule = _cacheService.GetData<ModuleDTO>(id.ToString());
            if (cachedModule != null)
                return cachedModule;

            var expiryTime = DateTimeOffset.Now.AddMinutes(60);

            var response = await _moduleRepository.Read(id);
            _cacheService.SetData<ModuleDTO>(id.ToString(), response, expiryTime);
            return response;
        }
        
        public async Task DeleteModule(Guid id)
        {
            _cacheService.RemoveData(id.ToString());
            await _moduleRepository.Delete(id);
        }
    }
}
