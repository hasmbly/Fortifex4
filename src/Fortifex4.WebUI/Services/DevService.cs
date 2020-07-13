using System.Net.Http;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Services
{
    public interface IDevService
    {
        public Task<ApiResponse<string>> GetFortifexOption(string subSection);
    }

    public class DevService : IDevService
    {
        private readonly HttpClient _httpClient;

        public DevService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<string>> GetFortifexOption(string subSection)
        {
            return await _httpClient.GetJsonAsync<ApiResponse<string>>($"{Constants.URI.Dev.GetFortifexOption}/{subSection}");
        }
    }
}