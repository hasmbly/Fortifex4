using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencyNames
{
    public class UpdateFiatCurrencyNamesResponse : GeneralResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public UpdateFiatCurrencyNamesResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}