using System.Collections.Generic;

namespace Fortifex4.Application.Wallets.Commands.SyncAllPersonalWallets
{
    public class PocketDTO
    {
        public int PocketID { get; set; }
        public int CurrencyID { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }

        public IList<TransactionDTO> Transactions { get; set; }

        public PocketDTO()
        {
            this.Transactions = new List<TransactionDTO>();
        }
    }
}
