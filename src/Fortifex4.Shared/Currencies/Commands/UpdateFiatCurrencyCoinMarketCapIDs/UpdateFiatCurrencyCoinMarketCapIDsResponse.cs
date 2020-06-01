using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencyCoinMarketCapIDs
{
    public class UpdateFiatCurrencyCoinMarketCapIDsResponse : GeneralResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public UpdateFiatCurrencyCoinMarketCapIDsResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}