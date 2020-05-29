using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies
{
    public class GetPreferableCoinCurrenciesResponse
    {
        public IList<CoinCurrencyDTO> CoinCurrencies { get; set; }

        public GetPreferableCoinCurrenciesResponse()
        {
            this.CoinCurrencies = new List<CoinCurrencyDTO>();
        }
    }
}