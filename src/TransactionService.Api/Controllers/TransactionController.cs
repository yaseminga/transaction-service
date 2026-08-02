using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.DTOs.Transactions;
using TransactionService.Application.Managers.Interfaces;

namespace TransactionService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionController(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> Create(CreateTransactionRequest request)
        {
            var transaction = await _transactionManager.CreateAsync(request);

            return Ok(transaction);
        }

        [HttpGet("high-volume")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetHighVolumeTransactions(
        [FromQuery] decimal threshold)
        {
            var transactions = await _transactionManager.GetHighVolumeTransactionsAsync(threshold);

            return Ok(transactions);
        }

        [HttpGet("summary/users")]
        public async Task<ActionResult<IEnumerable<UserTransactionSummaryResponse>>> GetUserSummary()
        {
            var summary = await _transactionManager.GetUserTransactionSummaryAsync();

            return Ok(summary);
        }

        [HttpGet("summary/types")]
        public async Task<ActionResult<IEnumerable<TransactionTypeSummaryResponse>>> GetTransactionTypeSummary()
        {
            var summary = await _transactionManager.GetTransactionTypeSummaryAsync();

            return Ok(summary);
        }
    }
}
