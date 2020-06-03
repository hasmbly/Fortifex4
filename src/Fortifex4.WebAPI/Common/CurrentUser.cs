using System.Security.Claims;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace Fortifex4.WebAPI.Common
{
    public class CurrentUser : ICurrentUser
    {
        public string Username { get; }
        public string PictureURL { get; }
        public bool IsAuthenticated { get; }

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            this.Username = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimType.Token);
            this.PictureURL = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimType.PictureUrl);
            this.IsAuthenticated = !string.IsNullOrEmpty(this.Username);
        }
    }
}