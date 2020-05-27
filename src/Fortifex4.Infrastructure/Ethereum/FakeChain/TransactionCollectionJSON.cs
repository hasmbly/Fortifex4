using System.Collections.Generic;

namespace Fortifex4.Infrastructure.Ethereum.FakeChain
{
    public class TransactionCollectionJSON
    {
        public IList<TransactionJSON> transactions { get; set; }

        public TransactionCollectionJSON()
        {
            this.transactions = new List<TransactionJSON>();
        }
    }

    public class TransactionJSON
    {
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public decimal amount { get; set; }
        public string hash { get; set; }
        public long timeStamp { get; set; }
    }
}