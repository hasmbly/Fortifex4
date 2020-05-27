namespace Fortifex4.Infrastructure.Fiat.Fixer
{
    public class LatestResultJSON
    {
        public bool success { get; set; }
        public long timestamp { get; set; }
        public FiatCurrenciesJSON rates { get; set; }
    }
}