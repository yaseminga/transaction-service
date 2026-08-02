namespace TransactionService.Domain.Entities
{
    public class User
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
