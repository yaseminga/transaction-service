using TransactionService.Domain.Enums;

namespace TransactionService.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}
