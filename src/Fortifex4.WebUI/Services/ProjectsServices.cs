using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Contributors.Commands.AcceptInvitation;
using Fortifex4.Shared.Contributors.Commands.CreateContributors;
using Fortifex4.Shared.Contributors.Commands.DeleteContributor;
using Fortifex4.Shared.Contributors.Commands.RejectInvitation;
using Fortifex4.Shared.Contributors.Commands.UpdateContributorInvitationStatus;
using Fortifex4.Shared.Contributors.Queries.GetContributorsByMemberUsername;
using Fortifex4.Shared.Projects.Commands.CreateProjects;
using Fortifex4.Shared.Projects.Commands.UpdateProjects;
using Fortifex4.Shared.Projects.Commands.UpdateProjectStatus;
using Fortifex4.Shared.Projects.Queries.GetMyProjects;
using Fortifex4.Shared.Projects.Queries.GetProject;
using Fortifex4.Shared.Projects.Queries.GetProjectsConfirmation;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IProjectsServices
    {
        public Task<ApiResponse<CreateProjectsResponse>> CreateProject(CreateProjectsRequest request);
        public Task<ApiResponse<UpdateProjectsResponse>> UpdateProject(UpdateProjectsRequest request);
        public Task<ApiResponse<DeleteContributorResponse>> DeleteContributors(DeleteContributorRequest request);
        public Task<ApiResponse<CreateContributorsResponse>> InviteMembers(CreateContributorsRequest request);

        public Task<ApiResponse<UpdateContributorInvitationStatusResponse>> UpdateInvitation(UpdateContributorInvitationStatusRequest request);
        public Task<ApiResponse<UpdateProjectStatusResponse>> UpdateProjectStatus(UpdateProjectStatusRequest request);
        public Task<ApiResponse<AcceptInvitationResponse>> AcceptProjectInvitation(string invitationCode);
        public Task<ApiResponse<RejectInvitationResponse>> RejectProjectInvitation(string invitationCode);

        public Task<ApiResponse<GetMyProjectsResponse>> GetMyProjects(string memberUsername);
        public Task<ApiResponse<GetProjectResponse>> GetProject(int projectID);
        public Task<ApiResponse<GetProjectResponse>> GetProjectIsExist(string memberUsername);
        public Task<ApiResponse<GetContributorsByMemberUsernameResponse>> GetContributorsByMemberUsername(string memberUsername);
        public Task<ApiResponse<GetProjectsConfirmationResponse>> GetProjectsConfirmation();
    }

    public class ProjectsServices : IProjectsServices
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public ProjectsServices(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<CreateProjectsResponse>> CreateProject(CreateProjectsRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<CreateProjectsResponse>>(Constants.URI.Projects.CreateProject, request);
        }

        public async Task<ApiResponse<UpdateProjectsResponse>> UpdateProject(UpdateProjectsRequest request)
        {
            await SetHeader();

            return await _httpClient.PutJsonAsync<ApiResponse<UpdateProjectsResponse>>(Constants.URI.Projects.UpdateProject, request);
        }

        public async Task<ApiResponse<DeleteContributorResponse>> DeleteContributors(DeleteContributorRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<DeleteContributorResponse>>(Constants.URI.Projects.DeleteContributors, request);
        }

        public async Task<ApiResponse<CreateContributorsResponse>> InviteMembers(CreateContributorsRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<CreateContributorsResponse>>(Constants.URI.Projects.InviteMembers, request);
        }

        public async Task<ApiResponse<UpdateContributorInvitationStatusResponse>> UpdateInvitation(UpdateContributorInvitationStatusRequest request)
        {
            await SetHeader();

            return await _httpClient.PutJsonAsync<ApiResponse<UpdateContributorInvitationStatusResponse>>(Constants.URI.Projects.UpdateInvitation, request);
        }

        public async Task<ApiResponse<UpdateProjectStatusResponse>> UpdateProjectStatus(UpdateProjectStatusRequest request)
        {
            await SetHeader();

            return await _httpClient.PutJsonAsync<ApiResponse<UpdateProjectStatusResponse>>(Constants.URI.Projects.UpdateProjectStatus, request);
        }

        public async Task<ApiResponse<AcceptInvitationResponse>> AcceptProjectInvitation(string invitationCode)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<AcceptInvitationResponse>>($"{Constants.URI.Projects.AcceptProjectInvitation}/{invitationCode}");
        }

        public async Task<ApiResponse<RejectInvitationResponse>> RejectProjectInvitation(string invitationCode)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<RejectInvitationResponse>>($"{Constants.URI.Projects.RejectProjectInvitation}/{invitationCode}");
        }

        public async Task<ApiResponse<GetMyProjectsResponse>> GetMyProjects(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetMyProjectsResponse>>($"{Constants.URI.Projects.GetMyProjects}/{memberUsername}");
        }

        public async Task<ApiResponse<GetContributorsByMemberUsernameResponse>> GetContributorsByMemberUsername(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetContributorsByMemberUsernameResponse>>($"{Constants.URI.Projects.GetContributorsByMemberUsername}/{memberUsername}");
        }

        public async Task<ApiResponse<GetProjectsConfirmationResponse>> GetProjectsConfirmation()
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetProjectsConfirmationResponse>>(Constants.URI.Projects.GetProjectsConfirmation);
        }

        public async Task<ApiResponse<GetProjectResponse>> GetProject(int projectID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetProjectResponse>>($"{Constants.URI.Projects.GetProject}/{projectID}");
        }

        public async Task<ApiResponse<GetProjectResponse>> GetProjectIsExist(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetProjectResponse>>($"{Constants.URI.Projects.GetProjectIsExist}/{memberUsername}");
        }
    }
}