using MediatR;

namespace Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet
{
    public class CreatePersonalWalletRequest : IRequest<CreatePersonalWalletResponse>
    {
        public string MemberUsername { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal? StartingBalance { get; set; }
    }
}