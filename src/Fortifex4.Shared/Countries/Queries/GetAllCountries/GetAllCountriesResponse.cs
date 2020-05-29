using System.Collections.Generic;

namespace Fortifex4.Shared.Countries.Queries.GetAllCountries
{
    public class GetAllCountriesResponse
    {
        public IList<CountryDTO> Countries { get; set; }

        public GetAllCountriesResponse()
        {
            this.Countries = new List<CountryDTO>();
        }
    }
}