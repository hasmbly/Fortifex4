using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains
{
    public class GetAllBlockchainsResponse : GeneralResponse
    {
        public IList<BlockchainDTO> Blockchains { get; set; }

        public GetAllBlockchainsResponse()
        {
            this.Blockchains = new List<BlockchainDTO>();
        }
    }
}