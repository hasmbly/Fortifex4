using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.Crypto
{
    public class CryptoBlockchain
    {
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Slug { get; set; }
        public int Rank { get; set; }

        public IList<CryptoCurrency> Currencies { get; set; }

        public CryptoBlockchain()
        {
            this.Currencies = new List<CryptoCurrency>();
        }
    }
}