namespace Fortifex4.Domain.Constants
{
    public static class ConfigurationKey
    {
        public static class AppSettings
        {
            public const string WalletSynchronizationMillisecondsDelay = "AppSettings:WalletSynchronizationMillisecondsDelay";
            public const string EmailServiceProvider = "AppSettings:EmailServiceProvider";
            public const string FiatServiceProvider = "AppSettings:FiatServiceProvider";
            public const string CryptoServiceProvider = "AppSettings:CryptoServiceProvider";
            public const string EthereumServiceProvider = "AppSettings:EthereumServiceProvider";
            public const string BitcoinServiceProvider = "AppSettings:BitcoinServiceProvider";
            public const string DogecoinServiceProvider = "AppSettings:DogecoinServiceProvider";
        }

        public static class ConnectionStrings
        {
            public const string FortifexDatabase = "ConnectionStrings:FortifexDatabase";
        }

        public static class Authentication
        {
            public static class Google
            {
                public const string ClientId = "Authentication:Google:ClientId";
                public const string ClientSecret = "Authentication:Google:ClientSecret";
            }

            public static class Facebook
            {
                public const string AppId = "Authentication:Facebook:AppId";
                public const string AppSecret = "Authentication:Facebook:AppSecret";
            }
        }

        public static class Email
        {
            public static class SendGrid
            {
                public const string APIKey = "Email:SendGrid:APIKey";
            }
        }

        public static class Fiat
        {
            public static class Fixer
            {
                public const string APIKey = "Fiat:Fixer:APIKey";
            }
        }

        public static class Crypto
        {
            public static class CoinMarketCap
            {
                public const string APIKey = "Crypto:CoinMarketCap:APIKey";
            }
        }

        public static class Ethereum
        {
            public static class Ethplorer
            {
                public const string APIKey = "Ethereum:Ethplorer:APIKey";
            }
        }
    }
}