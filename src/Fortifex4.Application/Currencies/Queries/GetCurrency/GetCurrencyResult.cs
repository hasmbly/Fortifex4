namespace Fortifex4.Application.Currencies.Queries.GetCurrency
{
    public class GetCurrencyResult
    {
        public int CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public string BlockchainName { get; set; }
    }
}