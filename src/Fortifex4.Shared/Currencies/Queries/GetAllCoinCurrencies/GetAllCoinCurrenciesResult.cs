using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Queries.GetAllCoinCurrencies
{
    public class GetAllCoinCurrenciesResult
    {
        public IList<CoinCurrencyDTO> CoinCurrencies { get; set; }

        public GetAllCoinCurrenciesResult()
        {
            this.CoinCurrencies = new List<CoinCurrencyDTO>();
        }
    }
}