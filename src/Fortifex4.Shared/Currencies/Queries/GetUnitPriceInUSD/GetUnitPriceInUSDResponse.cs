using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Queries.GetUnitPriceInUSD
{
    public class GetUnitPriceInUSDResponse : GeneralResponse
    {
        public decimal UnitPriceInUSD { get; set; } = 0m;
    }
}