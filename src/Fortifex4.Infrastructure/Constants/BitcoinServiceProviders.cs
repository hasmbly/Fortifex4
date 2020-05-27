namespace Fortifex4.Infrastructure.Constants
{
    public static class BitcoinServiceProviders
    {
        public static class BlockExplorer
        {
            public const string Name = "BlockExplorer";
            public const string AddrEndpointURL = "https://blockexplorer.com/api/addr";
        }

        public static class BitcoinChain
        {
            public const string Name = "BitcoinChain";
            public const string AddressEndpointURL = "https://api-r.bitcoinchain.com/v1/address";
            public const string AddressTxsEndpointURL = "https://api-r.bitcoinchain.com/v1/address/txs";
        }

        public static class SoChain
        {
            public const string Name = "SoChain";
            public const string GetAddressBalanceBTCEndpointURL = "https://sochain.com/api/v2/get_address_balance/BTC";
        }

        public static class FakeChain
        {
            public const string Name = "FakeChain";
            public const string GetAddressInfoEndpointURL = "https://fakechain.vioren.com/api/btc/getAddressInfo";
            public const string GetAddressTransactionsEndpointURL = "https://fakechain.vioren.com/api/btc/getAddressTransactions";
        }
    }
}