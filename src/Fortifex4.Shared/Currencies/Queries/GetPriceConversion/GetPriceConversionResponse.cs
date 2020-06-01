using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Queries.GetPriceConversion
{
    public class GetPriceConversionResponse : GeneralResponse
    {
        public decimal ConvertedAmount { get; set; } = 0m;
    }
}