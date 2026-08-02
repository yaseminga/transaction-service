using TransactionService.Domain.Enums;

namespace TransactionService.Application.DTOs.Transactions
{
    public class CreateTransactionRequest
    {
        public required string UserId { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }
    }
}
