using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Withdrawals.Commands.CreateWithdrawal
{
    public class CreateWithdrawalResponse : GeneralResponse
    {
        public int TransactionID { get; set; }
        public int WalletID { get; set; }
    }
}