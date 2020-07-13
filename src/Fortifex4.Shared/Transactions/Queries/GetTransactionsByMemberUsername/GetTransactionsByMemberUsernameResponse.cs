using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Transactions.Queries.GetTransactionsByMemberUsername
{
    public class GetTransactionsByMemberUsernameResponse : GeneralResponse
    {
        public IList<TransactionDTO> Transactions { get; set; }

        public GetTransactionsByMemberUsernameResponse()
        {
            this.Transactions = new List<TransactionDTO>();
        }
    }
}