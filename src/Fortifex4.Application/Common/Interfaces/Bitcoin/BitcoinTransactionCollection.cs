using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.Bitcoin
{
    public class BitcoinTransactionCollection
    {
        public IList<BitcoinTransaction> Transactions { get; set; }

        public BitcoinTransactionCollection()
        {
            this.Transactions = new List<BitcoinTransaction>();
        }
    }
}