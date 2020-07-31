using System.Security.Claims;
using Fortifex4.Shared.Common;
using Fortifex4.WebAPI.Models;
using Microsoft.AspNetCore.Authentication;

namespace Fortifex4.WebAPI.Common
{
    public static class PictureURLHelper
    {
        public static string GetPictureURL(AuthenticateResult authenticateResult)
        {
            string authenticationScheme = authenticateResult.Ticket.AuthenticationScheme;

            if (authenticationScheme == SchemeProvider.Google)
            {
                return GetGooglePictureUrl(authenticateResult.Ticket.Properties.Items[".Token.access_token"]);
            }
            else if (authenticationScheme == SchemeProvider.Facebook)
            {
                return GetFacebookPictureUrl(authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetGooglePictureUrl(string accessToken)
        {
            string uri = $"https://www.googleapis.com/oauth2/v2/userinfo?alt=json&access_token={accessToken}";

            GoogleUserInfo googleUserInfo = WebRequestHelper.Get<GoogleUserInfo>(uri);

            return googleUserInfo.picture;
        }

        private static string GetFacebookPictureUrl(string externalID)
        {
            return $"https://graph.facebook.com/" + externalID + "/picture?type=normal";
        }
    }
}