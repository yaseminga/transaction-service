using AutoMapper;
using TransactionService.Application.DTOs.Users;
using TransactionService.Application.Managers.Interfaces;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Repositories.Implementations;
using TransactionService.Infrastructure.Repositories.Interfaces;

namespace TransactionService.Application.Managers.Implementations
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserManager(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            user.Id = Guid.NewGuid().ToString();

            user = await _userRepository.AddAsync(user);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new Exception($"User is not found with id: {id}");
            }

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateAsync(string id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new Exception($"User is not found with id: {id}");
            }

            _mapper.Map(request, user);

            user = await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new Exception($"User is not found with id: {id}");
            }

            await _userRepository.DeleteAsync(user);
        }

    }
}
