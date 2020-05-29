using System.Collections.Generic;

namespace Fortifex4.Shared.Lookup.Queries.GetOwners
{
    public class GetOwnersResponse
    {
        public IList<OwnerDTO> Owners { get; set; }

        public GetOwnersResponse()
        {
            this.Owners = new List<OwnerDTO>();
        }
    }
}