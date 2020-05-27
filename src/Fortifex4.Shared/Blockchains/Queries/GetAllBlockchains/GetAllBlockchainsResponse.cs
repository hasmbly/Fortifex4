using System.Collections.Generic;

namespace Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains
{
    public class GetAllBlockchainsResponse
    {
        public IList<BlockchainDTO> Blockchains { get; set; }

        public GetAllBlockchainsResponse()
        {
            this.Blockchains = new List<BlockchainDTO>();
        }
    }
}