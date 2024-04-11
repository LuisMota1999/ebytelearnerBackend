using AutoMapper;
using ebyteLearner.DTOs.Course;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ebyteLearner.Data.Repository
{
    public interface IModuleRepository
    {
        Task<(int rowsAffected, ModuleDTO module)> Create(CreateModuleRequestDTO request);
        Task<ModuleDTO> Read(Guid id);
        Task<(int rowsAffected, ModuleDTO moduleDTO)> Update(Guid id, UpdateModuleRequestDTO request);
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

        public async Task<(int rowsAffected, ModuleDTO module)> Create(CreateModuleRequestDTO request)
        {
            if (request.ModuleName.IsNullOrEmpty())
                throw new AppException($"Module name cannot be empty");

            if (_dbContext.Module.Any(x => x.ModuleName == request.ModuleName))
                throw new AppException($"Module '{request.ModuleName}' is already registered");

            if (_dbContext.Course.Find(request.CourseID) == null)
                throw new AppException($"Course '{request.CourseID}' not found or does not exist");

            var module = _mapper.Map<Module>(request);

            _dbContext.Module.Add(module);
            try
            {
                var rowsAffected = await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Created module with ID: {module.Id}, rows affected: {rowsAffected}");

                return (rowsAffected, _mapper.Map<ModuleDTO>(module));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating module");
                throw;
            }
        }

        public async Task<ModuleDTO> Read(Guid id)
        {
            var moduleDB = await _dbContext.Module.FindAsync(id);
            if (moduleDB == null)
            {
                _logger.LogWarning($"Module with ID: {id} not found!");
                throw new AppException("Module '" + id + "' not found");
            }

            var courseResponse = _mapper.Map<ModuleDTO>(moduleDB);
            return courseResponse;
        }

        public async Task<(int rowsAffected, ModuleDTO moduleDTO)> Update(Guid id, UpdateModuleRequestDTO request)
        {

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var moduleDB = await _dbContext.Module.FindAsync(id);
            
            if (moduleDB != null)
            {
                moduleDB.ModuleName = request.ModuleName ?? moduleDB.ModuleName;
                moduleDB.ModuleDescription = request.ModuleDescription ?? moduleDB.ModuleDescription;
                moduleDB.isFree = request.isFree ?? moduleDB.isFree;
                
                if (request.ModulePDFId != Guid.Empty)
                {
                    moduleDB.ModulePDFId = request.ModulePDFId!;
                }
                try
                {
                    var rowsAffected = await _dbContext.SaveChangesAsync();
                    _logger.LogInformation($"Updated Course with ID: {moduleDB.Id}, rows affected: {rowsAffected}");
                    return (rowsAffected, _mapper.Map<ModuleDTO>(moduleDB));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw new AppException("Concurrency conflict occurred while updating the module.", ex);
                }
                catch (DbUpdateException ex)
                {
                    throw new AppException("Error occurred while updating the module in the database.", ex);
                }
            }
            else
            {
                throw new AppException($"Module with ID '{id}' not found.");
            }

            
        }

        public async Task Delete(Guid id)
        {
            var module = await _dbContext.Course.FindAsync(id);
            if (module != null)
            {
                _dbContext.Entry(module).State = EntityState.Deleted;
                _dbContext.Remove(module);
                await _dbContext.SaveChangesAsync();
            }
            else
                throw new AppException("Module '" + id + "' not found");
        }
    }
}