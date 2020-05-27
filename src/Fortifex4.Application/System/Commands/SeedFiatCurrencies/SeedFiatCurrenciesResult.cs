namespace Fortifex4.Application.System.Commands.SeedFiatCurrencies
{
    public class SeedFiatCurrenciesResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public int FiatCurrenciesAdded { get; set; }
    }
}