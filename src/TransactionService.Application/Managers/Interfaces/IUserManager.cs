using TransactionService.Application.DTOs.Users;

namespace TransactionService.Application.Managers.Interfaces
{
    public interface IUserManager
    {
        Task<UserResponse> CreateAsync(CreateUserRequest request);

        Task<IEnumerable<UserResponse>> GetAllAsync();

        Task<UserResponse> GetByIdAsync(string id);

        Task<UserResponse> UpdateAsync(string id, UpdateUserRequest request);

        Task DeleteAsync(string id);
    }
}
