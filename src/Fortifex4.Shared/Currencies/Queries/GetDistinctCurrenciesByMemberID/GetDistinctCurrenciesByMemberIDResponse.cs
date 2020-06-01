using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Queries.GetDistinctCurrenciesByMemberID
{
    public class GetDistinctCurrenciesByMemberIDResponse : GeneralResponse
    {
        public IList<CurrencyDTO> Currencies { get; set; }

        public GetDistinctCurrenciesByMemberIDResponse()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}