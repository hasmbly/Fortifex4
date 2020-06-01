using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies
{
    public class GetPreferableCoinCurrenciesResponse : GeneralResponse
    {
        public IList<CoinCurrencyDTO> CoinCurrencies { get; set; }

        public GetPreferableCoinCurrenciesResponse()
        {
            this.CoinCurrencies = new List<CoinCurrencyDTO>();
        }
    }
}