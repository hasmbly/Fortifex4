using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces.Dogecoin
{
    public class DogecoinTransactionCollection
    {
        public IList<DogecoinTransaction> Transactions { get; set; }

        public DogecoinTransactionCollection()
        {
            this.Transactions = new List<DogecoinTransaction>();
        }
    }
}