using System.Collections.Generic;

namespace Fortifex4.Shared.Transactions.Queries.GetTransactionsByMemberUsername
{
    public class GetTransactionsByMemberUsernameResponse
    {
        public IList<TransactionDTO> Transactions { get; set; }

        public GetTransactionsByMemberUsernameResponse()
        {
            this.Transactions = new List<TransactionDTO>();
        }
    }
}