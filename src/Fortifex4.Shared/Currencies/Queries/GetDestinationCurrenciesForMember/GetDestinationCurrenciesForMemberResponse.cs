using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class GetDestinationCurrenciesForMemberResponse : GeneralResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetDestinationCurrenciesForMemberResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}