using MediatR;

namespace Fortifex4.Shared.Currencies.Queries.GetPriceConversion
{
    public class GetPriceConversionRequest : IRequest<GetPriceConversionResponse>
    {
        public string FromCurrencySymbol { get; set; }
        public string ToCurrencySymbol { get; set; }
        public decimal Amount { get; set; }
    }
}