using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Countries.Queries.GetAllCountries
{
    public class GetAllCountriesResponse : GeneralResponse
    {
        public IList<CountryDTO> Countries { get; set; }

        public GetAllCountriesResponse()
        {
            this.Countries = new List<CountryDTO>();
        }
    }
}