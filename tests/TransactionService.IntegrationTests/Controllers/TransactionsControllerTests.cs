using FluentAssertions;
using System.Net.Http.Json;
using TransactionService.Application.DTOs.Transactions;
using TransactionService.Application.DTOs.Users;
using TransactionService.Domain.Enums;
using TransactionService.IntegrationTests.Fixtures;

namespace TransactionService.IntegrationTests.Controllers
{
    public class TransactionsControllerTests : IntegrationTestBase
    {
        public TransactionsControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {

        }

        private async Task<UserResponse> CreateUserAsync()
        {
            var response = await Client.PostAsJsonAsync("/api/users",
                new CreateUserRequest
                {
                    Name = "Yasemin"
                });

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<UserResponse>())!;
        }

        [Fact]
        public async Task CreateTransaction_ShouldCreateTransaction()
        {
            await Factory.ResetDatabaseAsync();

            var user = await CreateUserAsync();

            var request = new CreateTransactionRequest
            {
                UserId = user.Id,
                Amount = 1500,
                Type = TransactionType.Debit
            };

            var response = await Client.PostAsJsonAsync("/api/transactions", request);

            response.EnsureSuccessStatusCode();

            var transaction = await response.Content.ReadFromJsonAsync<TransactionResponse>();

            transaction.Should().NotBeNull();
            transaction!.Amount.Should().Be(1500);
            transaction.UserId.Should().Be(user.Id);
        }

        [Fact]
        public async Task HighVolumeTransactions_ShouldReturnOrderedList()
        {
            await Factory.ResetDatabaseAsync();

            var user = await CreateUserAsync();

            await Client.PostAsJsonAsync("/api/transactions",
                new CreateTransactionRequest
                {
                    UserId = user.Id,
                    Amount = 300,
                    Type = TransactionType.Debit
                });

            await Client.PostAsJsonAsync("/api/transactions",
                new CreateTransactionRequest
                {
                    UserId = user.Id,
                    Amount = 2000,
                    Type = TransactionType.Debit
                });

            await Client.PostAsJsonAsync("/api/transactions",
                new CreateTransactionRequest
                {
                    UserId = user.Id,
                    Amount = 1500,
                    Type = TransactionType.Credit
                });

            var response = await Client.GetAsync("/api/transactions/high-volume?threshold=500");

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<List<TransactionResponse>>();

            result.Should().HaveCount(2);

            result.Should().BeInDescendingOrder(x => x.Amount);

            result.Should().OnlyContain(x => x.Amount >= 500);
        }

        [Fact]
        public async Task UserSummary_ShouldReturnTotalAmountPerUser()
        {
            await Factory.ResetDatabaseAsync();

            var user = await CreateUserAsync();

            await Client.PostAsJsonAsync("/api/transactions",
                new CreateTransactionRequest
                {
                    UserId = user.Id,
                    Amount = 100,
                    Type = TransactionType.Debit
                });

            await Client.PostAsJsonAsync("/api/transactions",
                new CreateTransactionRequest
                {
                    UserId = user.Id,
                    Amount = 250,
                    Type = TransactionType.Credit
                });

            var response = await Client.GetAsync("/api/transactions/summary/users");

            response.EnsureSuccessStatusCode();

            var report =
                await response.Content.ReadFromJsonAsync<List<UserTransactionSummaryResponse>>();

            report.Should().ContainSingle();

            report!.First().TotalAmount.Should().Be(350);
        }

        [Fact]
        public async Task TransactionTypeSummary_ShouldReturnTotalAmountPerType()
        {
            await Factory.ResetDatabaseAsync();

            var user = await CreateUserAsync();

            await Client.PostAsJsonAsync("/api/transactions",
                new CreateTransactionRequest
                {
                    UserId = user.Id,
                    Amount = 200,
                    Type = TransactionType.Debit
                });

            await Client.PostAsJsonAsync("/api/transactions",
                new CreateTransactionRequest
                {
                    UserId = user.Id,
                    Amount = 500,
                    Type = TransactionType.Debit
                });

            await Client.PostAsJsonAsync("/api/transactions",
                new CreateTransactionRequest
                {
                    UserId = user.Id,
                    Amount = 300,
                    Type = TransactionType.Credit
                });

            var response = await Client.GetAsync("/api/transactions/summary/types");

            response.EnsureSuccessStatusCode();

            var report =
                await response.Content.ReadFromJsonAsync<List<TransactionTypeSummaryResponse>>();

            report.Should().Contain(x =>
                x.Type == TransactionType.Debit &&
                x.TotalAmount == 700);

            report.Should().Contain(x =>
                x.Type == TransactionType.Credit &&
                x.TotalAmount == 300);
        }

    }
}
