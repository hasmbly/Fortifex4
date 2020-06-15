using Fortifex4.Shared.Wallets.Common;

namespace Fortifex4.Shared.Wallets.Commands.SyncPersonalWallet
{
    public class SyncPersonalWalletResponse
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }

        public WalletDTO Wallet { get; set; }

        public SyncPersonalWalletResponse()
        {
            this.Wallet = new WalletDTO();
        }
    }
}