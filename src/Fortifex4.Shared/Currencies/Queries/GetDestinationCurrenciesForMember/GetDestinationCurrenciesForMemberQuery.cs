using MediatR;

namespace Fortifex4.Application.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class GetDestinationCurrenciesForMemberQuery : IRequest<GetDestinationCurrenciesForMemberResult>
    {
        public string MemberUsername { get; set; }
    }
}