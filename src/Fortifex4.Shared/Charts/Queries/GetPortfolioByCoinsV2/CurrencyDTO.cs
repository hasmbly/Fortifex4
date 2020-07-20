using System.Collections.Generic;
using System.Linq;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Shared.Charts.Queries.GetPortfolioByCoinsV2
{
    public class CurrencyDTO
    {
        public int CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public decimal Price { get; set; }
        public decimal TotalAmount => this.TransactionDTO.Sum(x => x.Amount);
        public decimal CurrentValueInPreferredFiatCurrency => this.TotalAmount * this.Price;

        public IList<TransactionDTO> TransactionDTO { get; set; }

        public CurrencyDTO()
        {
            this.TransactionDTO = new List<TransactionDTO>();
        }
    }
}