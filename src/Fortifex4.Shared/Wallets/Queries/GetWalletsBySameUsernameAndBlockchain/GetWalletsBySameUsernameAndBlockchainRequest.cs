using MediatR;

namespace Fortifex4.Shared.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain
{
    public class GetWalletsBySameUsernameAndBlockchainRequest : IRequest<GetWalletsBySameUsernameAndBlockchainResponse>
    {
        public int WalletID { get; set; }
    }
}