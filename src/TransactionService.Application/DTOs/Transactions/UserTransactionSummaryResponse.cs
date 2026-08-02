using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionService.Application.DTOs.Transactions
{
    public class UserTransactionSummaryResponse
    {
        public required string UserId { get; set; }

        public required string UserName { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
