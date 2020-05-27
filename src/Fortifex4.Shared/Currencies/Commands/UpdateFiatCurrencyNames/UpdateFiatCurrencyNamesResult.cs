using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Commands.UpdateFiatCurrencyNames
{
    public class UpdateFiatCurrencyNamesResult
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public UpdateFiatCurrencyNamesResult()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}