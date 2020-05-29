using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencies
{
    public class UpdateFiatCurrenciesResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public UpdateFiatCurrenciesResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}