namespace Fortifex4.WebUI
{
    public static class Constants
    {
        public const string Bearer = "Bearer";

        public static class URI
        {
            public const string BaseAPI = "api";

            public static class Account
            {
                public static readonly string CheckUsername = $"{BaseAPI}/account/checkUsername";
                public static readonly string Login = $"{BaseAPI}/account/login";
                public static readonly string GetMember = $"{BaseAPI}/account/getMember";
                public static readonly string ActivateMember = $"{BaseAPI}/account/activate?code=";
            }

            public static class Members
            {
                public static readonly string CreateMember = $"{BaseAPI}/members/createMember";
                public static readonly string UpdateMember = $"{BaseAPI}/members/updateMember";

                public static readonly string GetPreferences = $"{BaseAPI}/members/getPreferences";
                public static readonly string UpdatePreferredTimeFrame = $"{BaseAPI}/members/updatePreferredTimeFrame";
                public static readonly string UpdatePreferredCoinCurrency = $"{BaseAPI}/members/updatePreferredCoinCurrency";
                public static readonly string UpdatePreferredFiatCurrency = $"{BaseAPI}/members/updatePreferredFiatCurrency";
            }

            public static class TimeFrames
            {
                public static readonly string GetAllTimeFrames = $"{BaseAPI}/timeFrames/getAllTimeFrames";
            }

            public static class Genders
            {
                public static readonly string GetAllGenders = $"{BaseAPI}/genders/getAllGenders";
            }

            public static class Regions
            {
                public static readonly string GetRegions = $"{BaseAPI}/regions/getRegions";
            }

            public static class Blockchains
            {
                public static readonly string GetAllBlockchains = $"{BaseAPI}/blockchains/getAllBlockchains";
            }

            public static class Countries
            {
                public static readonly string GetAllCountries = $"{BaseAPI}/countries/getAllCountries";
            }

            public static class Currencies
            {
                public static readonly string GetAllFiatCurrencies = $"{BaseAPI}/currencies/getAllFiatCurrencies";
                public static readonly string GetPreferableCoinCurrencies = $"{BaseAPI}/currencies/getPreferableCoinCurrencies";
            }

            public static class Wallets
            {
                public static readonly string CreatePersonalWallet = $"{BaseAPI}/wallets/createPersonalWallet";
                public static readonly string UpdatePersonalWallet = $"{BaseAPI}/wallets/updatePersonalWallet";
                public static readonly string DeleteWallet = $"{BaseAPI}/wallets/deleteWallet";
                public static readonly string GetPersonalWallets = $"{BaseAPI}/wallets/getPersonalWallets";
                public static readonly string GetWallet = $"{BaseAPI}/wallets/getWallet";
                public static readonly string SyncPersonalWallet = $"{BaseAPI}/wallets/syncPersonalWallet";
            }

            public static class InternalTransfers
            {
                public static readonly string GetInternalTransfer = $"{BaseAPI}/internalTransfers/getInternalTransfer";
                public static readonly string CreateInternalTransfer = $"{BaseAPI}/internalTransfers/createInternalTransfer";
                public static readonly string UpdateInternalTransfer = $"{BaseAPI}/internalTransfers/updateInternalTransfer";
                public static readonly string DeleteInternalTransfer = $"{BaseAPI}/internalTransfers/deleteInternalTransfer";
                public static readonly string GetWalletsWithSameCurrency = $"{BaseAPI}/internalTransfers/getWalletsWithSameCurrency";
                public static readonly string GetAllWalletsWithSameCurrency = $"{BaseAPI}/internalTransfers/getAllWalletsWithSameCurrency";
            }
        }

        public static class AlertMessageStatus
        {
            public const string Success = "alert-success";
            public const string Warning = "alert-warning";
        }

        public static class StorageKey
        {
            public const string Token = "Token";
        }

        public static class AuthenticationType
        {
            public const string ServerAuthentication = "ServerAuthentication";
        }
    }
}