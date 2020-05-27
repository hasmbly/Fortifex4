using System.Collections.Generic;

namespace Fortifex4.Application.Genders.Queries.GetAllGenders
{
    public class GetAllGendersResult
    {
        public IEnumerable<GenderDTO> Genders { get; set; }
    }
}