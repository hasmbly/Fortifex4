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

            public static class Currencies
            {
                public static readonly string GetAllFiatCurrencies = $"{BaseAPI}/currencies/getAllFiatCurrencies";
                public static readonly string GetPreferableCoinCurrencies = $"{BaseAPI}/currencies/getPreferableCoinCurrencies";
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