using System.Collections.Generic;

namespace Fortifex4.Shared.System.Commands.SeedMasterData
{
    public class SeedMasterDataResponse
    {
        public IDictionary<string, int> Entities { get; set; }

        public SeedMasterDataResponse()
        {
            this.Entities = new Dictionary<string, int>();
        }
    }
}