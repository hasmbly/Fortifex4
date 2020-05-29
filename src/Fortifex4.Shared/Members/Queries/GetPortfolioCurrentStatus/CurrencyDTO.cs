using System.Collections.Generic;
using System.Linq;

namespace Fortifex4.Shared.Members.Queries.GetPortfolioCurrentStatus
{
    public class CurrencyDTO
    {
        public int CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public decimal CurrentUnitPriceInPreferredFiatCurrency { get; set; }
        public decimal CurrentValueInPreferredCoinCurrency { get; set; }

        public IList<TransactionDTO> Transactions { get; set; }

        public CurrencyDTO()
        {
            this.Transactions = new List<TransactionDTO>();
        }

        public string CurrentValueInPreferredCoinCurrencyDisplayText
        {
            get
            {
                return this.CurrentValueInPreferredCoinCurrency.ToString("N4").Replace(".0000", "");
            }
        }

        public decimal CurrentValueInPreferredFiatCurrency
        {
            get
            {
                return this.TotalAmount * this.CurrentUnitPriceInPreferredFiatCurrency;
            }
        }

        public decimal TotalAmount
        {
            get
            {
                return this.Transactions.Sum(x => x.Amount);
            }
        }
    }
}