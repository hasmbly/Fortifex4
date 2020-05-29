namespace Fortifex4.Shared.Currencies.Queries.GetCurrency
{
    public class GetCurrencyResponse
    {
        public int CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public string BlockchainName { get; set; }
    }
}