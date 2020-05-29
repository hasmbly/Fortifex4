namespace Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies
{
    public class CoinCurrencyDTO
    {
        public int CurrencyID { get; set; }
        public int BlockchainID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public bool IsShownInTradePair { get; set; }
        public bool IsForPreferredOption { get; set; }

        public string NameWithSymbol
        {
            get
            {
                return $"{this.Name} ({this.Symbol})";
            }
        }
    }
}