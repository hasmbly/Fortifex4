using System.Collections.Generic;
using System.Linq;

namespace Fortifex4.Application.Pockets.Queries.GetPocket
{
    public class GetPocketResult
    {
        public int PocketID { get; set; }
        public int WalletID { get; set; }
        public string Address { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string WalletName { get; set; }
        public string WalletBlockchainName { get; set; }

        public IList<TransactionDTO> Transactions { get; set; }

        public GetPocketResult()
        {
            this.Transactions = new List<TransactionDTO>();
        }

        public decimal Balance
        {
            get
            {
                return this.Transactions.Sum(x => x.Amount);
            }
        }

        public string BalanceDisplayText
        {
            get
            {
                return this.Balance.ToString("N4");
            }
        }
    }
}