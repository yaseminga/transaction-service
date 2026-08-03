using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.DTOs.Transactions;
using TransactionService.Application.Managers.Interfaces;

namespace TransactionService.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for creating transactions and transaction reports.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionsController(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        /// <summary>
        /// Creates a new transaction.
        /// </summary>
        /// <param name="request">The transaction information.</param>
        /// <returns>The newly created transaction.</returns>
        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> Create(CreateTransactionRequest request)
        {
            var transaction = await _transactionManager.CreateAsync(request);

            return Ok(transaction);
        }

        /// <summary>
        /// Retrieves transactions whose amount exceeds the specified threshold.
        /// </summary>
        /// <param name="threshold">The minimum transaction amount.</param>
        /// <returns>A collection of high-volume transactions.</returns>
        [HttpGet("high-volume")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetHighVolumeTransactions(
        [FromQuery] decimal threshold)
        {
            var transactions = await _transactionManager.GetHighVolumeTransactionsAsync(threshold);

            return Ok(transactions);
        }

        /// <summary>
        /// Returns the total transaction amount grouped by user.
        /// </summary>
        /// <returns>A summary report for all users.</returns>
        [HttpGet("summary/users")]
        public async Task<ActionResult<IEnumerable<UserTransactionSummaryResponse>>> GetUserSummary()
        {
            var summary = await _transactionManager.GetUserTransactionSummaryAsync();

            return Ok(summary);
        }

        /// <summary>
        /// Generates a report containing the total transaction amount for each transaction type.
        /// </summary>
        /// <returns>A collection of transaction summaries grouped by transaction type.</returns>
        [HttpGet("summary/types")]
        public async Task<ActionResult<IEnumerable<TransactionTypeSummaryResponse>>> GetTransactionTypeSummary()
        {
            var summary = await _transactionManager.GetTransactionTypeSummaryAsync();

            return Ok(summary);
        }
    }
}
