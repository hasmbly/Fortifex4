using System.Collections.Generic;

namespace Fortifex4.Shared.Genders.Queries.GetAllGenders
{
    public class GetAllGendersResponse
    {
        public IList<GenderDTO> Genders { get; set; }

        public GetAllGendersResponse()
        {
            this.Genders = new List<GenderDTO>();
        }
    }
}