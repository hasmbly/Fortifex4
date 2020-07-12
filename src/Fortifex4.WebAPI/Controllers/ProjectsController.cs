using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Contributors.Commands.AcceptInvitation;
using Fortifex4.Shared.Contributors.Commands.CreateContributors;
using Fortifex4.Shared.Contributors.Commands.DeleteContributor;
using Fortifex4.Shared.Contributors.Commands.RejectInvitation;
using Fortifex4.Shared.Contributors.Commands.UpdateContributorInvitationStatus;
using Fortifex4.Shared.InternalTransfers.Commands.CreateInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Commands.DeleteInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Commands.UpdateInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Queries.GetInternalTransfer;
using Fortifex4.Shared.Projects.Commands.CreateProjects;
using Fortifex4.Shared.Projects.Commands.UpdateProjects;
using Fortifex4.Shared.Projects.Commands.UpdateProjectStatus;
using Fortifex4.Shared.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain;
using Fortifex4.Shared.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class ProjectsController : ApiController
    {
        [Authorize]
        [HttpGet("getInternalTransfer/{internalTransferID}")]
        public async Task<IActionResult> GetInternalTransfer(int internalTransferID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetInternalTransferRequest() { InternalTransferID = internalTransferID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

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
    }
}