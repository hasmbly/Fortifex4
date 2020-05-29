namespace Fortifex4.Shared.System.Commands.SeedFiatCurrencies
{
    public class SeedFiatCurrenciesResponse
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public int FiatCurrenciesAdded { get; set; }
    }
}