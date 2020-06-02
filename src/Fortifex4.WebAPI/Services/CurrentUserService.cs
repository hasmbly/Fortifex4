using System.Security.Claims;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace Fortifex4.WebAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public bool IsAuthenticated { get; }
        public string Username { get; }
        public string PictureURL { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Username = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            PictureURL = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimType.PictureUrl);
            IsAuthenticated = Username != null;
        }
    }
}