namespace Fortifex4.Application.Currencies.Queries.GetAvailableCurrencies
{
    public class CurrencyDTO
    {
        public int CurrencyID { get; set; }
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