using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class GetDestinationCurrenciesForMemberResult
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetDestinationCurrenciesForMemberResult()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}