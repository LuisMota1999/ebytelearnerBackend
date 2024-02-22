using AutoMapper;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Helpers;
using ebyteLearner.Models;

namespace ebyteLearner.Data.Repository
{
    public interface IModuleRepository
    {
        Task Create(CreateModuleRequestDTO request);
        Task<ModuleDTO> Read(Guid id);
        Task<ModuleDTO> Update(Guid id, UpdateModuleRequestDTO request);     
        Task Delete(Guid id);
        
    }

    public class ModuleRepository : IModuleRepository       
    {
        private readonly DBContextService _dbContext;
        private readonly ILogger<ModuleRepository> _logger;
        private readonly IMapper _mapper;
        public ModuleRepository(DBContextService dbContext, ILogger<ModuleRepository> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Create(CreateModuleRequestDTO request)
        {
            if (_dbContext.Module.Any(x => x.ModuleName == request.ModuleName))
                throw new AppException("Module '" + request.ModuleName + "' is already registered");

            if (_dbContext.Course.Find(request.CourseID) == null)
                throw new AppException("Course '" + request.CourseID + "' not found or do not exist");

            var module = _mapper.Map<Module>(request);

            _dbContext.Module.Add(module);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ModuleDTO> Read(Guid id)
        {
            var moduleDB = await _dbContext.Module.FindAsync(id);
            if (moduleDB != null)
            {
                var courseResponse = _mapper.Map<ModuleDTO>(moduleDB);
                return courseResponse;
            }
            else
                throw new AppException("Module '" + id + "' not found");
        }

        public async Task<ModuleDTO> Update(Guid id, UpdateModuleRequestDTO request)
        {
            var moduleDB = await _dbContext.Module.FindAsync(id);
            if (moduleDB != null)
            {
                _mapper.Map(request, moduleDB);
                await _dbContext.SaveChangesAsync();
                var updatedModule = _mapper.Map<ModuleDTO>(moduleDB);

                return updatedModule;
            }
            else
                throw new AppException("Module '" + id + "' not found");
        }

        public async Task Delete(Guid id)
        {
            var module = await _dbContext.Course.FindAsync(id);
            if (module != null)
            {
                _dbContext.Remove(module);
                 _dbContext.SaveChanges();
            }
            else
                throw new AppException("Module '" + id + "' not found");
        }
    }
}