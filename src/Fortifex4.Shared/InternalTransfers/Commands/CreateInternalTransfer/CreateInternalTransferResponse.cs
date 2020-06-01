using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.InternalTransfers.Commands.CreateInternalTransfer
{
    public class CreateInternalTransferResponse : GeneralResponse
    {
        public int InternalTransferID { get; set; }
        public int WalletID { get; set; }
    }
}