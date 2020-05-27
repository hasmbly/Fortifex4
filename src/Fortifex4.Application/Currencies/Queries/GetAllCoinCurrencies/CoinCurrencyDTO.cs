using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Currencies.Queries.GetAllCoinCurrencies
{
    public class CoinCurrencyDTO
    {
        public int CurrencyID { get; set; }
        public string BlockchainName { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public string NameWithSymbol
        {
            get
            {
                return $"{this.Name} ({this.Symbol})";
            }
        }
    }
}