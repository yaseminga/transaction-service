using TransactionService.Domain.Enums;

namespace TransactionService.Application.DTOs.Transactions
{
    public class TransactionResponse
    {
        public int Id { get; set; }

        public required string UserId { get; set; }

        public required string UserName { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
