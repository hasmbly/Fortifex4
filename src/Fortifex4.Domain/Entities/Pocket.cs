using Fortifex4.Domain.Common;
using Fortifex4.Domain.Enums;
using System.Collections.Generic;

namespace Fortifex4.Domain.Entities
{
    public class Pocket : AuditableEntity
    {
        public int PocketID { get; set; }
        public int WalletID { get; set; }
        public int CurrencyID { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string Address { get; set; }
        public bool IsMain { get; set; }

        public Wallet Wallet { get; set; }
        public Currency Currency { get; set; }

        public IList<Transaction> Transactions { get; private set; }

        public Pocket()
        {
            this.Transactions = new List<Transaction>();
        }
    }
}