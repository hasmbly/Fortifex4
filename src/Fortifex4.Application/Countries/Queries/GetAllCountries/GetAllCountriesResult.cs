using System.Collections.Generic;

namespace Fortifex4.Application.Countries.Queries.GetAllCountries
{
    public class GetAllCountriesResult
    {
        public IList<CountryDTO> Countries { get; set; }

        public GetAllCountriesResult()
        {
            this.Countries = new List<CountryDTO>();
        }
    }
}