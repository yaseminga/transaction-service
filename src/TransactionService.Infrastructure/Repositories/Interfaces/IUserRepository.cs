using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);

        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(string id);

        Task<User> UpdateAsync(User user);

        Task DeleteAsync(User user);
    }
}
