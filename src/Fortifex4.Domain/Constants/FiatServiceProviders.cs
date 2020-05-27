namespace Fortifex4.Domain.Constants
{
    public static class FiatServiceProviders
    {
        public static class Fixer
        {
            public const string Name = "Fixer";
            public const string ConvertEndpointURL = "http://data.fixer.io/api/convert";
            public const string LatestEndpointURL = "http://data.fixer.io/api/latest";
            public const string SymbolsEndpointURL = "http://data.fixer.io/api/symbols";
        }
    }
}