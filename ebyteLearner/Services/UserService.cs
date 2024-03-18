using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.User;
using ebyteLearner.Models;

namespace ebyteLearner.Services
{

    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> SearchUser(string searchQuery, int page = 1, int pageSize = 10);
        void UpdateUser(Guid Id, UpdateUserRequestDTO request);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<IEnumerable<UserDTO>> GetActiveTeacherUsers();
        Task<UserDTO> GetUser(Guid id);
        Task DeleteUser(Guid id);
    }
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void UpdateUser(Guid Id, UpdateUserRequestDTO request)
        {

            _userRepository.Update(Id, request);


        }

        public async Task<IEnumerable<UserDTO>> SearchUser(string searchQuery, int page = 1, int pageSize = 10)
        {
            var users = await _userRepository.SearchUsers(searchQuery)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return users;
        }


        public async Task<UserDTO> GetUser(Guid id)
        {
            return await _userRepository.Read(id);
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            return await _userRepository.ReadAllUsers().ToListAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            await _userRepository.Delete(id);
        }

        public async Task<IEnumerable<UserDTO>> GetActiveTeacherUsers()
        {
            return await _userRepository.GetActiveTeacherUsers().ToListAsync();
        }

    }

}
