using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.System.Commands.RemoveAllTransactions
{
    public class RemoveAllTransactionsResponse : GeneralResponse
    {
        public int TransactionsRemoved { get; set; }
    }
}