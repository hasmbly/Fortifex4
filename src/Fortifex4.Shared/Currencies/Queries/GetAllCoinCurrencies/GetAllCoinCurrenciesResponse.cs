using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Queries.GetAllCoinCurrencies
{
    public class GetAllCoinCurrenciesResponse : GeneralResponse
    {
        public IList<CoinCurrencyDTO> CoinCurrencies { get; set; }

        public GetAllCoinCurrenciesResponse()
        {
            this.CoinCurrencies = new List<CoinCurrencyDTO>();
        }
    }
}