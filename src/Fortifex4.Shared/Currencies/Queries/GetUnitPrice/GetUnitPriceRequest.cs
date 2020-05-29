using MediatR;

namespace Fortifex4.Shared.Currencies.Queries.GetUnitPrice
{
    public class GetUnitPriceRequest : IRequest<GetUnitPriceResponse>
    {
        public string FromCurrencySymbol { get; set; }
        public string ToCurrencySymbol { get; set; }
    }
}