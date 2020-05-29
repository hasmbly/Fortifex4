namespace Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies
{
    public class FiatCurrencyDTO
    {
        public int CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public bool IsShownInTradePair { get; set; }
        public bool IsForPreferredOption { get; set; }
        public decimal UnitPriceInUSD { get; set; }

        public string NameWithSymbol
        {
            get
            {
                return $"{this.Name} ({this.Symbol})";
            }
        }
    }
}