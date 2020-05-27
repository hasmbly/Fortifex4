using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.Crypto
{
    public class CryptoBlockchainCollection
    {
        public IList<CryptoBlockchain> Blockchains { get; set; }

        public CryptoBlockchainCollection()
        {
            this.Blockchains = new List<CryptoBlockchain>();
        }
    }
}