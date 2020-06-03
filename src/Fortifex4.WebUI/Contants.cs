namespace Fortifex4.WebUI
{
    public static class Constants
    {
        public const string Bearer = "Bearer";

        public static class URI
        {
            public static class Account
            {
                public static readonly string CheckUsername = $"account/checkUsername";
                public static readonly string Login = $"account/login";
                public static readonly string GetMember = $"account/getMember";
            }

            public static class Members
            {
                public static readonly string CreateMember = $"members/createMember";
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