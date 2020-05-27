using System;
using System.Collections.Generic;

namespace Fortifex4.Infrastructure.Ethereum.Ethplorer
{
    public class WalletJSON
    {
        public string address { get; set; }
        public ETHJSON ETH { get; set; }
        public IList<TokenJSON> tokens { get; set; }

        public WalletJSON()
        {
            this.tokens = new List<TokenJSON>();
        }
    }

    public class ETHJSON
    {
        public decimal balance { get; set; }
    }

    public class TokenJSON
    {
        public TokenInfoJSON tokenInfo { get; set; }
        public decimal balance { get; set; }

        public decimal CalculatedBalance
        {
            get
            {
                decimal balance = this.balance;

                if (this.tokenInfo != null)
                {
                    balance = this.balance / (decimal)Math.Pow(10, this.tokenInfo.decimals);
                }

                return balance;
            }
        }
    }

    public class TokenInfoJSON
    {
        public string address { get; set; }
        public string name { get; set; }
        public int decimals { get; set; }
        public string symbol { get; set; }
        public long lastUpdated { get; set; }
    }
}