using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Queries.GetPreferrableCoinCurrencies
{
    public class GetPreferableCoinCurrenciesResult
    {
        public IList<CoinCurrencyDTO> CoinCurrencies { get; set; }

        public GetPreferableCoinCurrenciesResult()
        {
            this.CoinCurrencies = new List<CoinCurrencyDTO>();
        }
    }
}