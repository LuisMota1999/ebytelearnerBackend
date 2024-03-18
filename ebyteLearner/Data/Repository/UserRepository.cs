using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ebyteLearner.DTOs.User;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using ebyteLearner.DTOs.Course;

namespace ebyteLearner.Data.Repository
{
    public interface IUserRepository
    {
        Task Update(Guid id, UpdateUserRequestDTO request);
        Task<UserDTO> Read(Guid id);
        IQueryable<UserDTO> ReadAllUsers();
        IQueryable<UserDTO> SearchUsers(string searchQuery);
        IQueryable<UserDTO> GetActiveTeacherUsers();
        Task Delete(Guid id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DBContextService _dbContext;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMapper _mapper;
        public UserRepository(DBContextService dbContext, ILogger<UserRepository> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Update(Guid id, UpdateUserRequestDTO request)
        {
            var userDB = await _dbContext.User.FindAsync(id);
            if (userDB != null)
            {
                _mapper.Map(request, userDB);
                _dbContext.Entry(userDB).State = EntityState.Modified;
                
                try
                {
                    await _dbContext.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw new AppException("Concurrency conflict occurred while updating the user.", ex);
                }
                catch (DbUpdateException ex)
                {
                    throw new AppException("Error occurred while updating the user in the database.", ex);
                }
            }
            else
            {
                throw new AppException("User '" + id + "' not found");
            }
        }

        public async Task<UserDTO> Read(Guid id)
        {
            var userDB = await _dbContext.User.FindAsync(id);
            if (userDB != null)
            {
                var userResponse = _mapper.Map<UserDTO>(userDB);
                return userResponse;
            }
            else
                throw new AppException("User '" + id + "' not found");
        }

        public async Task Delete(Guid id)
        {
            var user = await _dbContext.User.FindAsync(id);
            if (user != null)
            {
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new AppException($"User '{id}' not found");
            }
        }

        public IQueryable<UserDTO> ReadAllUsers()
        {
            return _dbContext.User.Select(u => _mapper.Map<UserDTO>(u));
        }

        public IQueryable<UserDTO> SearchUsers(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                throw new ArgumentException("Search query cannot be empty");
            }

            searchQuery = SanitizeSearchQuery(searchQuery);

            return _dbContext.User
                .Where(u => EF.Functions.Like(u.Username, $"%{searchQuery}%"))
                .Select(u => _mapper.Map<UserDTO>(u));
        }

        public IQueryable<UserDTO> GetActiveTeacherUsers()
        {
            return _dbContext.User
                .Where(u => u.UserRole == UserRole.Teacher)
                .Select(u => _mapper.Map<UserDTO>(u));
        }

        private string SanitizeSearchQuery(string searchQuery)
        {
            return searchQuery;
        }
    }
}