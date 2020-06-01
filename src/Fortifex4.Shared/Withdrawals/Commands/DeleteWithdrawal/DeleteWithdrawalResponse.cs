using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Withdrawals.Commands.DeleteWithdrawal
{
    public class DeleteWithdrawalResponse : GeneralResponse
    {
        public int WalletID { get; set; }
    }
}