using TransactionService.Domain.Enums;

namespace TransactionService.Application.DTOs.Transactions
{
    public class TransactionTypeSummaryResponse
    {
        public TransactionType Type { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
