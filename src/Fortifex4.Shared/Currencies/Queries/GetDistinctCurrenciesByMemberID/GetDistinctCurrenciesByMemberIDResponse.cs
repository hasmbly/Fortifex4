using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Queries.GetDistinctCurrenciesByMemberID
{
    public class GetDistinctCurrenciesByMemberIDResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetDistinctCurrenciesByMemberIDResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}