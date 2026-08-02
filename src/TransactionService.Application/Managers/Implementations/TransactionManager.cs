using AutoMapper;
using TransactionService.Application.DTOs.Transactions;
using TransactionService.Application.Managers.Interfaces;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Repositories.Interfaces;

namespace TransactionService.Application.Managers.Implementations
{
    public class TransactionManager : ITransactionManager
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionManager(IUserRepository userRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                throw new Exception($"User is not found with id: {request.UserId}");
            }

            var transaction = _mapper.Map<Transaction>(request);
            transaction.User = user;
            transaction.CreatedAt = DateTime.UtcNow;

            transaction = await _transactionRepository.AddAsync(transaction);

            return _mapper.Map<TransactionResponse>(transaction);
        }

        public async Task<IEnumerable<TransactionResponse>> GetHighVolumeTransactionsAsync(decimal threshold)
        {
            var transactions = await _transactionRepository
                .GetHighVolumeTransactionsAsync(threshold);

            return _mapper.Map<IEnumerable<TransactionResponse>>(transactions);
        }

        public async Task<IEnumerable<TransactionTypeSummaryResponse>> GetTransactionTypeSummaryAsync()
        {
            var summaries = await _transactionRepository
                .GetTransactionTypeSummaryAsync();

            return _mapper.Map<IEnumerable<TransactionTypeSummaryResponse>>(summaries);
        }

        public async Task<IEnumerable<UserTransactionSummaryResponse>> GetUserTransactionSummaryAsync()
        {
            var summaries = await _transactionRepository
            .GetUserTransactionSummaryAsync();

            return _mapper.Map<IEnumerable<UserTransactionSummaryResponse>>(summaries);
        }
    }
}
