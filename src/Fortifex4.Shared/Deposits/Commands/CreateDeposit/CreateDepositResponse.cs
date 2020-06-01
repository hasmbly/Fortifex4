using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Deposits.Commands.CreateDeposit
{
    public class CreateDepositResponse : GeneralResponse
    {
        public int TransactionID { get; set; }
        public int PocketID { get; set; }
        public int WalletID { get; set; }
    }
}