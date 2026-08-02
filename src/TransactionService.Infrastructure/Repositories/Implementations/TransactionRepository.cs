using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.QueryModels;
using TransactionService.Infrastructure.Data;
using TransactionService.Infrastructure.Repositories.Interfaces;

namespace TransactionService.Infrastructure.Repositories.Implementations
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _context;

        public TransactionRepository(TransactionDbContext context)
        {
            _context = context;
        }
        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);

            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetHighVolumeTransactionsAsync(decimal threshold)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(x => x.Amount > threshold)
                .OrderByDescending(x => x.Amount)
                .Include(x => x.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<TransactionTypeSummary>> GetTransactionTypeSummaryAsync()
        {
            return await _context.Transactions
                .AsNoTracking()
                .GroupBy(x => x.Type)
                .Select(g => new TransactionTypeSummary
                {
                    Type = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTransactionSummary>> GetUserTransactionSummaryAsync()
        {
            return await _context.Transactions
                .AsNoTracking()
                .GroupBy(x => new
                {
                    x.UserId,
                    x.User.Name
                })
                .Select(g => new UserTransactionSummary
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.Name,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToListAsync();
        }
    }
}
