using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Queries.GetDistinctCurrenciesByMemberID
{
    public class GetDistinctCurrenciesByMemberIDResult
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetDistinctCurrenciesByMemberIDResult()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}