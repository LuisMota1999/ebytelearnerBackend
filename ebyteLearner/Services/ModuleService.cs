using AutoMapper;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ebyteLearner.Services
{
    public interface IModuleService
    {
        Task<(int rows, ModuleDTO module)> CreateModule(CreateModuleRequestDTO request);
        Task<ModuleDTO> GetModule(Guid id);
        Task<(int rows, ModuleDTO moduleDTO)> UpdateModule(Guid Id, UpdateModuleRequestDTO request);
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

        public async Task<(int rows, ModuleDTO module)> CreateModule(CreateModuleRequestDTO request)
        {
            var (rows, response) = await _moduleRepository.Create(request);
            return (rows, response);
        }
        public async Task<(int rows, ModuleDTO moduleDTO)> UpdateModule(Guid Id, UpdateModuleRequestDTO request)
        {

            var cachedModules = _cacheService.GetData<IEnumerable<ModuleDTO>>("GetAllModules");
            if (cachedModules != null)
                _cacheService.RemoveData("GetAllModules");
            var (rows, response) = await _moduleRepository.Update(Id, request);
            return (rows, response);
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
