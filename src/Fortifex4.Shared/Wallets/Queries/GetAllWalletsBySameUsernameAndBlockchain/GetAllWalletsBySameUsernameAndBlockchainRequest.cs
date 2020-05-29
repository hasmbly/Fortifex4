using MediatR;

namespace Fortifex4.Shared.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain
{
    public class GetAllWalletsBySameUsernameAndBlockchainRequest : IRequest<GetAllWalletsBySameUsernameAndBlockchainResponse>
    {
        public string MemberUsername { get; set; }
    }
}