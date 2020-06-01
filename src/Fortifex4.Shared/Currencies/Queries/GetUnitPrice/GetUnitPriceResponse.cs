using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Queries.GetUnitPrice
{
    public class GetUnitPriceResponse : GeneralResponse
    {
        public decimal UnitPrice { get; set; } = 0m;
    }
}