using Fortifex4.Domain.Enums;
using System.Collections.Generic;

namespace Fortifex4.Application.Wallets.Queries.GetWallet
{
    public class PocketDTO
    {
        public int PocketID { get; set; }
        public int WalletID { get; set; }
        public int CurrencyID { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }

        public IList<TransactionDTO> Transactions { get; set; }

        public PocketDTO()
        {
            this.Transactions = new List<TransactionDTO>();
        }
    }
}