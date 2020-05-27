using System;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Members.Queries.GetPortfolioCurrentStatus
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
    }
}