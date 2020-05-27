namespace Fortifex4.Infrastructure.Constants
{
    public static class EthereumServiceProviders
    {
        public static class Ethplorer
        {
            public const string Name = "Ethplorer";
            public const string GetAddressInfoEndpointURL = "https://api.ethplorer.io/getAddressInfo";
            public const string GetAddressTransactionsEndpointURL = "https://api.ethplorer.io/getAddressTransactions";
        }

        public static class FakeChain
        {
            public const string Name = "FakeChain";
            public const string GetAddressInfoEndpointURL = "https://fakechain.vioren.com/api/eth/getAddressInfo";
            public const string GetAddressTransactionsEndpointURL = "https://fakechain.vioren.com/api/eth/getAddressTransactions";
        }
    }
}