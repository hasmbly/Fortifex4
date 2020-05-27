using System.Collections.Generic;

namespace Fortifex4.Infrastructure.Ethereum.FakeChain
{
    public class WalletJSON
    {
        public string address { get; set; }
        public decimal balance { get; set; }
        public int transactionsCount { get; set; }
        public IList<TokenJSON> tokens { get; set; }

        public WalletJSON()
        {
            this.tokens = new List<TokenJSON>();
        }
    }

    public class TokenJSON
    {
        public string address { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public decimal balance { get; set; }
    }
}