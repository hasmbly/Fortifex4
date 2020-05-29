using System.Collections.Generic;
using System.Linq;

namespace Fortifex4.Shared.Members.Queries.GetPortfolioCurrentStatus
{
    public class GetPortfolioCurrentStatusResponse
    {
        public string MemberPreferredFiatCurrencySymbol { get; set; }
        public string MemberPreferredCoinCurrencySymbol { get; set; }

        public IList<CurrencyDTO> Currencies { get; set; }

        public GetPortfolioCurrentStatusResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }

        public decimal TotalValueInFiatCurrency
        {
            get
            {
                return this.Currencies.Sum(x => x.CurrentValueInPreferredFiatCurrency);
            }
        }

        public string TotalValueInFiatCurrencyDisplayText
        {
            get
            {
                return this.TotalValueInFiatCurrency.ToString("N2");
            }
        }

        public decimal TotalValueInCoinCurrency
        {
            get
            {
                return this.Currencies.Sum(x => x.CurrentValueInPreferredCoinCurrency);
            }
        }

        public string TotalValueInCoinCurrencyDisplayText
        {
            get
            {
                return this.TotalValueInCoinCurrency.ToString("N2");
            }
        }
    }
}