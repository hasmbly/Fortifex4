using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Queries.GetAllCoinCurrencies
{
    public class GetAllCoinCurrenciesResponse
    {
        public IList<CoinCurrencyDTO> CoinCurrencies { get; set; }

        public GetAllCoinCurrenciesResponse()
        {
            this.CoinCurrencies = new List<CoinCurrencyDTO>();
        }
    }
}