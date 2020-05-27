using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Commands.UpdateFiatCurrencies
{
    public class UpdateFiatCurrenciesResult
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public UpdateFiatCurrenciesResult()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}