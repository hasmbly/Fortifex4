using Fortifex4.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Fortifex4.WebAPI.Common
{
    public class CurrentWeb : ICurrentWeb
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentWeb(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string BaseURL => $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
    }
}