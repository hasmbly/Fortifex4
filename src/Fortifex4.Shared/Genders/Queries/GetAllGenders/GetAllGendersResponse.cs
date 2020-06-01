using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Genders.Queries.GetAllGenders
{
    public class GetAllGendersResponse : GeneralResponse
    {
        public IList<GenderDTO> Genders { get; set; }

        public GetAllGendersResponse()
        {
            this.Genders = new List<GenderDTO>();
        }
    }
}