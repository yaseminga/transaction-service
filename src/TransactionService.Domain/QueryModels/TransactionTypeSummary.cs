using TransactionService.Domain.Enums;

namespace TransactionService.Domain.QueryModels
{
    public class TransactionTypeSummary
    {
        public TransactionType Type { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
