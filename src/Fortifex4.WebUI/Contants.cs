namespace Fortifex4.WebUI
{
    public static class Constants
    {
        public const string Bearer = "Bearer";

        public static class URI
        {
            public static class Account
            {
                public static readonly string CheckUsername = $"api/account/checkUsername";
                public static readonly string Login = $"api/account/login";
                public static readonly string GetMember = $"api/account/getMember";
                public static readonly string ActivateMember = $"api/account/activate?code=";
            }

            public static class Members
            {
                public static readonly string CreateMember = $"api/members/createMember";
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