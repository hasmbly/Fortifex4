using System.Collections.Generic;

namespace Fortifex4.Shared.Charts.Queries.GetPortfolioByCoinsV2
{
    public class GetPortfolioByCoinsV2Response
    {
        public string MemberPreferredFiatCurrencySymbol { get; set; }
        public decimal MemberPreferredFiatCurrencyUnitPriceInUSD { get; set; }
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetPortfolioByCoinsV2Response()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}