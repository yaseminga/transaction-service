using TransactionService.Application.DTOs.Transactions;

namespace TransactionService.Application.Managers.Interfaces
{
    public interface ITransactionManager
    {
        Task<TransactionResponse> CreateAsync(CreateTransactionRequest request);

        Task<IEnumerable<TransactionResponse>> GetHighVolumeTransactionsAsync(decimal threshold);

        Task<IEnumerable<UserTransactionSummaryResponse>> GetUserTransactionSummaryAsync();

        Task<IEnumerable<TransactionTypeSummaryResponse>> GetTransactionTypeSummaryAsync();
    }
}
