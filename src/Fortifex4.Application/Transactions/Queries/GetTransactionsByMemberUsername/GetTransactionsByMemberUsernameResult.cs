using System.Collections.Generic;

namespace Fortifex4.Application.Transactions.Queries.GetTransactionsByMemberUsername
{
    public class GetTransactionsByMemberUsernameResult
    {
        public IList<TransactionDTO> Transactions { get; set; }

        public GetTransactionsByMemberUsernameResult()
        {
            this.Transactions = new List<TransactionDTO>();
        }
    }
}