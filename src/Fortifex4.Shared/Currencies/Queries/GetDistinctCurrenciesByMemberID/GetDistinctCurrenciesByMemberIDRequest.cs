using MediatR;

namespace Fortifex4.Shared.Currencies.Queries.GetDistinctCurrenciesByMemberID
{
    public class GetDistinctCurrenciesByMemberIDRequest : IRequest<GetDistinctCurrenciesByMemberIDResponse>
    {
        public string MemberUsername { get; set; }
    }
}