using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Lookup.Queries.GetOwners
{
    public class GetOwnersResponse : GeneralResponse
    {
        public IList<OwnerDTO> Owners { get; set; }

        public GetOwnersResponse()
        {
            this.Owners = new List<OwnerDTO>();
        }
    }
}