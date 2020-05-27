using System.Collections.Generic;

namespace Fortifex4.Application.System.Commands.SeedMasterData
{
    public class SeedMasterDataResult
    {
        public IDictionary<string, int> Entities { get; set; }

        public SeedMasterDataResult()
        {
            this.Entities = new Dictionary<string, int>();
        }
    }
}