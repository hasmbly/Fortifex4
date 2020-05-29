using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencyNames
{
    public class UpdateFiatCurrencyNamesResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public UpdateFiatCurrencyNamesResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}