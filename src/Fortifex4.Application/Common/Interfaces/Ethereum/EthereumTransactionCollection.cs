using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.Ethereum
{
    public class EthereumTransactionCollection
    {
        public IList<EthereumTransaction> Transactions { get; set; }

        public EthereumTransactionCollection()
        {
            this.Transactions = new List<EthereumTransaction>();
        }
    }
}