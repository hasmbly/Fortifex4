using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.ProjectDocuments.Commands.DeleteProjectDocument;
using Fortifex4.Shared.ProjectDocuments.Commands.UpdateProjectDocument;
using Fortifex4.Shared.ProjectDocuments.Queries.GetProjectDocument;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IProjectsDocumentService
    {
        public Task<ApiResponse<GetProjectDocumentResponse>> GetProjectDocument(int projectDocumentID);
        public Task<HttpResponseMessage> GetProjectDocumentDownload(int projectDocumentID);
        public Task<HttpResponseMessage> CreateProjectDocument(HttpContent request);
        public Task<HttpResponseMessage> UpdateProjectDocument(HttpContent request);
        public Task<ApiResponse<DeleteProjectDocumentResponse>> DeleteProjectDocument(DeleteProjectDocumentRequest request);
    }

    public class ProjectsDocumentService : IProjectsDocumentService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public ProjectsDocumentService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetProjectDocumentResponse>> GetProjectDocument(int projectDocumentID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetProjectDocumentResponse>>($"{Constants.URI.ProjectsDocument.GetProjectDocument}/{projectDocumentID}");
        }

        public async Task<HttpResponseMessage> GetProjectDocumentDownload(int projectDocumentID)
        {
            await SetHeader();

            return await _httpClient.GetAsync($"{Constants.URI.ProjectsDocument.GetProjectDocumentDownload}/{projectDocumentID}");
        }

        public async Task<HttpResponseMessage> CreateProjectDocument(HttpContent request)
        {
            await SetHeader();

            return await _httpClient.PostAsync(Constants.URI.ProjectsDocument.CreateProjectDocument, request);
        }

        public async Task<HttpResponseMessage> UpdateProjectDocument(HttpContent request)
        {
            await SetHeader();

            return await _httpClient.PutAsync(Constants.URI.ProjectsDocument.UpdateProjectDocument, request);
        }

        public async Task<ApiResponse<DeleteProjectDocumentResponse>> DeleteProjectDocument(DeleteProjectDocumentRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<DeleteProjectDocumentResponse>>(Constants.URI.ProjectsDocument.DeleteProjectDocument, request);
        }
    }
}