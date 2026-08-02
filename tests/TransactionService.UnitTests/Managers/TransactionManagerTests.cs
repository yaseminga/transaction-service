using AutoMapper;
using FluentAssertions;
using Moq;
using TransactionService.Application.DTOs.Transactions;
using TransactionService.Application.Managers.Implementations;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Enums;
using TransactionService.Domain.QueryModels;
using TransactionService.Infrastructure.Repositories.Interfaces;

namespace TransactionService.UnitTests.Managers
{
    public class TransactionManagerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TransactionManager _transactionManager;

        private const string UserId = "358142be-984b-4304-8c4f-c1364f1477cd";

        public TransactionManagerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();

            _transactionManager = new TransactionManager(
                _userRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTransaction_WhenUserExists()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                UserId = UserId,
                Amount = 1500,
                Type = TransactionType.Debit
            };

            var user = new User
            {
                Id = UserId,
                Name = "Yasemin"
            };

            var transaction = new Transaction
            {
                UserId = UserId,
                Amount = request.Amount,
                Type = request.Type
            };

            Transaction? savedTransaction = null;

            var response = new TransactionResponse
            {
                Id = 1,
                UserId = UserId,
                UserName = user.Name,
                Amount = request.Amount,
                Type = request.Type
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(UserId))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(x => x.Map<Transaction>(request))
                .Returns(transaction);

            _transactionRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(t => savedTransaction = t)
                .ReturnsAsync((Transaction t) => t);

            _mapperMock
                .Setup(x => x.Map<TransactionResponse>(It.IsAny<Transaction>()))
                .Returns(response);

            // Act
            var result = await _transactionManager.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(UserId);
            result.Amount.Should().Be(request.Amount);

            savedTransaction.Should().NotBeNull();
            savedTransaction!.User.Should().Be(user);
            savedTransaction.CreatedAt.Should()
                .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

            _userRepositoryMock.Verify(
                x => x.GetByIdAsync(UserId),
                Times.Once);

            _transactionRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Transaction>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                UserId = UserId,
                Amount = 1500,
                Type = TransactionType.Credit
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(UserId))
                .ReturnsAsync((User?)null);

            // Act
            Func<Task> action = () => _transactionManager.CreateAsync(request);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>();

            _transactionRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Transaction>()),
                Times.Never);
        }

        [Fact]
        public async Task GetHighVolumeTransactionsAsync_ShouldReturnTransactionsOrderedByAmount()
        {
            // Arrange
            const decimal threshold = 500;

            var transactions = new List<Transaction>
            {
                new()
                {
                    Id = 1,
                    UserId = UserId,
                    Amount = 2000,
                    Type = TransactionType.Credit,

                },
                new()
                {
                    Id = 2,
                    UserId = UserId,
                    Amount = 1500,
                    Type = TransactionType.Debit
                }
            };

            var response = new List<TransactionResponse>
            {
                new()
                {
                    Id = 1,
                    UserId = UserId,
                    UserName = "User 1",
                    Amount = 2000,
                    Type = TransactionType.Credit
                },
                new()
                {
                    Id = 2,
                    UserId = UserId,
                    UserName = "User 2",
                    Amount = 1500,
                    Type = TransactionType.Debit
                }
            };

            _transactionRepositoryMock
                .Setup(x => x.GetHighVolumeTransactionsAsync(threshold))
                .ReturnsAsync(transactions);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<TransactionResponse>>(transactions))
                .Returns(response);

            // Act
            var result = await _transactionManager
                .GetHighVolumeTransactionsAsync(threshold);

            // Assert
            result.Should().HaveCount(2);

            result.First().Amount.Should().BeGreaterThan(result.Last().Amount);

            _transactionRepositoryMock.Verify(
                x => x.GetHighVolumeTransactionsAsync(threshold),
                Times.Once);
        }

        [Fact]
        public async Task GetUserTransactionSummaryAsync_ShouldReturnUserSummaries()
        {
            // Arrange
            var summaries = new List<UserTransactionSummary>
            {
                new()
                {
                    UserId = UserId,
                    UserName = "Yasemin",
                    TotalAmount = 2500
                }
            };

            var response = new List<UserTransactionSummaryResponse>
            {
                new()
                {
                    UserId = UserId,
                    UserName = "Yasemin",
                    TotalAmount = 2500
                }
            };

            _transactionRepositoryMock
                .Setup(x => x.GetUserTransactionSummaryAsync())
                .ReturnsAsync(summaries);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<UserTransactionSummaryResponse>>(summaries))
                .Returns(response);

            // Act
            var result = await _transactionManager
                .GetUserTransactionSummaryAsync();

            // Assert
            result.Should().HaveCount(1);

            result.First().UserName.Should().Be("Yasemin");

            _transactionRepositoryMock.Verify(
                x => x.GetUserTransactionSummaryAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetTransactionTypeSummaryAsync_ShouldReturnTransactionTypeSummaries()
        {
            // Arrange
            var summaries = new List<TransactionTypeSummary>
            {
                new()
                {
                    Type = TransactionType.Credit,
                    TotalAmount = 5000
                }
            };

            var response = new List<TransactionTypeSummaryResponse>
            {
                new()
                {
                    Type = TransactionType.Debit,
                    TotalAmount = 15000
                }
            };

            _transactionRepositoryMock
                .Setup(x => x.GetTransactionTypeSummaryAsync())
                .ReturnsAsync(summaries);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<TransactionTypeSummaryResponse>>(summaries))
                .Returns(response);

            // Act
            var result = await _transactionManager
                .GetTransactionTypeSummaryAsync();

            // Assert
            result.Should().ContainSingle();

            result.First().Type.Should().Be(TransactionType.Debit);

            _transactionRepositoryMock.Verify(
                x => x.GetTransactionTypeSummaryAsync(),
                Times.Once);
        }
    }
}
