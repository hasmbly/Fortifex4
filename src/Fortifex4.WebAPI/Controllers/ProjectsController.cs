using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Contributors.Commands.AcceptInvitation;
using Fortifex4.Shared.Contributors.Commands.CreateContributors;
using Fortifex4.Shared.Contributors.Commands.DeleteContributor;
using Fortifex4.Shared.Contributors.Commands.RejectInvitation;
using Fortifex4.Shared.Contributors.Commands.UpdateContributorInvitationStatus;
using Fortifex4.Shared.Contributors.Queries.CheckIsContributor;
using Fortifex4.Shared.Contributors.Queries.GetContributorsByMemberUsername;
using Fortifex4.Shared.Projects.Commands.CreateProjects;
using Fortifex4.Shared.Projects.Commands.UpdateProjects;
using Fortifex4.Shared.Projects.Commands.UpdateProjectStatus;
using Fortifex4.Shared.Projects.Queries.GetMyProjects;
using Fortifex4.Shared.Projects.Queries.GetProject;
using Fortifex4.Shared.Projects.Queries.GetProjectsConfirmation;
using Fortifex4.Shared.ProjectStatusLogs.Queries.GetProjectStatusLogsByProjectID;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class ProjectsController : ApiController
    {
        [AllowAnonymous]
        [HttpPost("createProject")]
        public async Task<ActionResult> CreateProject(CreateProjectsRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPut("updateProject")]
        public async Task<IActionResult> UpdateProject(UpdateProjectsRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPost("deleteContributors")]
        public async Task<IActionResult> DeleteContributors(DeleteContributorRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpPost("inviteMembers")]
        public async Task<ActionResult> InviteMembers(CreateContributorsRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPut("updateInvitation")]
        public async Task<IActionResult> UpdateInvitation(UpdateContributorInvitationStatusRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPut("updateProjectStatus")]
        public async Task<IActionResult> UpdateProjectStatus(UpdateProjectStatusRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("accept/{invitationCode}")]
        public async Task<IActionResult> AcceptProjectInvitation(string invitationCode)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new AcceptInvitationRequest() { InvitationCode = invitationCode })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("reject/{invitationCode}")]
        public async Task<IActionResult> RejectProjectInvitation(string invitationCode)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new RejectInvitationRequest() { InvitationCode = invitationCode })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getMyProjects/{memberUsername}")]
        public async Task<IActionResult> GetMyProjects(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetMyProjectsRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getProjectIsExist/{isExistProjectByMemberUsername}")]
        public async Task<IActionResult> GetProjectIsExist(string isExistProjectByMemberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetProjectRequest()
                {
                    IsExistProjectByMemberUsername = isExistProjectByMemberUsername 
                })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getProject/{projectID}")]
        public async Task<IActionResult> GetProjectIsExist(int projectID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetProjectRequest() { ProjectID = projectID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getContributorsByMemberUsername/{memberUsername}")]
        public async Task<IActionResult> GetContributorsByMemberUsername(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetContributorsByMemberUsernameRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getProjectsConfirmation")]
        public async Task<IActionResult> GetProjectsConfirmation()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetProjectsConfirmationRequest() { } )));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getProjectStatusLogsByProjectID/{projectID}")]
        public async Task<IActionResult> GetProjectStatusLogsByProjectID(int projectID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetProjectStatusLogsByProjectIDRequest() { ProjectID = projectID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("checkIsContributor")]
        public async Task<IActionResult> CheckIsContributor(int projectID, string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new CheckIsContributorRequest() { ProjectID = projectID, MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}