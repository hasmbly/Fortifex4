using System.Collections.Generic;

namespace Fortifex4.Application.Lookup.Queries.GetOwners
{
    public class GetOwnersResult
    {
        public IList<OwnerDTO> Owners { get; set; }

        public GetOwnersResult()
        {
            this.Owners = new List<OwnerDTO>();
        }
    }
}