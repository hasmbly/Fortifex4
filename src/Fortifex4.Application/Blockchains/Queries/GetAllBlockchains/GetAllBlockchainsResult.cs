using System.Collections.Generic;

namespace Fortifex4.Application.Blockchains.Queries.GetAllBlockchains
{
    public class GetAllBlockchainsResult
    {
        public IList<BlockchainDTO> Blockchains { get; set; }

        public GetAllBlockchainsResult()
        {
            this.Blockchains = new List<BlockchainDTO>();
        }
    }
}