using MediatR;

namespace Fortifex4.Shared.Wallets.Commands.SyncPersonalWallet
{
    public class SyncPersonalWalletRequest : IRequest<SyncPersonalWalletResponse>
    {
        public int WalletID { get; set; }
    }
}