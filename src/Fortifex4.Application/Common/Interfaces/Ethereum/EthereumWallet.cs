using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.Ethereum
{
    public class EthereumWallet
    {
        public decimal Balance { get; set; }
        public IList<Token> Tokens { get; set; }

        public EthereumWallet()
        {
            this.Tokens = new List<Token>();
        }
    }

    public class Token
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal Balance { get; set; }
    }
}