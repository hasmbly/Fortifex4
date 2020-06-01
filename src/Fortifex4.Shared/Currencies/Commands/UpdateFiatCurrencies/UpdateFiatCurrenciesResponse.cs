using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencies
{
    public class UpdateFiatCurrenciesResponse : GeneralResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public UpdateFiatCurrenciesResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}