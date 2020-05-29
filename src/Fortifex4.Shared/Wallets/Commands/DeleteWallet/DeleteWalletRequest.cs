using MediatR;

namespace Fortifex4.Shared.Wallets.Commands.DeleteWallet
{
    public class DeleteWalletRequest : IRequest<DeleteWalletResponse>
    {
        public int WalletID { get; set; }
    }
}