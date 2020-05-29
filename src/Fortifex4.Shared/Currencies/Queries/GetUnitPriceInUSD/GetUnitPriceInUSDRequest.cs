using MediatR;

namespace Fortifex4.Shared.Currencies.Queries.GetUnitPriceInUSD
{
    public class GetUnitPriceInUSDRequest : IRequest<GetUnitPriceInUSDResponse>
    {
        public string CurrencySymbol { get; set; }
    }
}