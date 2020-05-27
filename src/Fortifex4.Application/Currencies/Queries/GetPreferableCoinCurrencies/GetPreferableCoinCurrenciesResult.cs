using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Queries.GetPreferrableCoinCurrencies
{
    public class GetPreferableCoinCurrenciesResult
    {
        public IEnumerable<CoinCurrencyDTO> CoinCurrencies { get; set; }
    }
}