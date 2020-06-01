using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Wallets.Commands.CreateExternalTransfer
{
    public class CreateExternalTransferResponse : GeneralResponse
    {
        public int TransactionID { get; set; }
    }
}