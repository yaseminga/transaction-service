namespace TransactionService.Domain.QueryModels
{
    public class UserTransactionSummary
    {
        public required string UserId { get; set; }

        public required string UserName { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
