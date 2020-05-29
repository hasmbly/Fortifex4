using MediatR;

namespace Fortifex4.Shared.Wallets.Commands.UpdatePersonalWallet
{
    public class UpdatePersonalWalletRequest : IRequest<UpdatePersonalWalletResponse>
    {
        public int WalletID { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}