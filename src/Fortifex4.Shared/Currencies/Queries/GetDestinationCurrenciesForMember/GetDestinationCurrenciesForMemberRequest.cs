using MediatR;

namespace Fortifex4.Shared.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class GetDestinationCurrenciesForMemberRequest : IRequest<GetDestinationCurrenciesForMemberResponse>
    {
        public string MemberUsername { get; set; }
    }
}