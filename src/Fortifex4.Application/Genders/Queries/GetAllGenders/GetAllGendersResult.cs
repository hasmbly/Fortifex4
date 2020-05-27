using System.Collections.Generic;

namespace Fortifex4.Application.Genders.Queries.GetAllGenders
{
    public class GetAllGendersResult
    {
        public IList<GenderDTO> Genders { get; set; }

        public GetAllGendersResult()
        {
            this.Genders = new List<GenderDTO>();
        }
    }
}