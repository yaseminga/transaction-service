using TransactionService.Domain.Entities;
using TransactionService.Domain.QueryModels;

namespace TransactionService.Infrastructure.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction transaction);

        Task<IEnumerable<Transaction>> GetHighVolumeTransactionsAsync(decimal threshold);

        Task<IEnumerable<UserTransactionSummary>> GetUserTransactionSummaryAsync();

        Task<IEnumerable<TransactionTypeSummary>> GetTransactionTypeSummaryAsync();
    }
}
