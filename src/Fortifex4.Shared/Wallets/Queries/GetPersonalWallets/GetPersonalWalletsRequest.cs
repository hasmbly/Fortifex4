using MediatR;

namespace Fortifex4.Shared.Wallets.Queries.GetPersonalWallets
{
    public class GetPersonalWalletsRequest : IRequest<GetPersonalWalletsResponse>
    {
        public string MemberUsername { get; set; }
    }
}