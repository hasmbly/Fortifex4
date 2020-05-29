using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class GetDestinationCurrenciesForMemberResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetDestinationCurrenciesForMemberResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}