using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Queries.GetDistinctCurrenciesByMemberID
{
    public class GetDistinctCurrenciesByMemberIDResult
    {
        public IEnumerable<CurrencyDTO> Currencies { get; set; }
    }
}