using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class GetDestinationCurrenciesForMemberResult
    {
        public IEnumerable<CurrencyDTO> Currencies { get; set; }
    }
}