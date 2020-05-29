using MediatR;

namespace Fortifex4.Shared.Wallets.Queries.GetWallet
{
    public class GetWalletRequest : IRequest<GetWalletResponse>
    {
        public int WalletID { get; set; }
    }
}