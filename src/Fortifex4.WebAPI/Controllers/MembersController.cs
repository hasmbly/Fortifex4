using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Shared.Members.Commands.CreateMember;
using Fortifex4.Shared.Members.Commands.UpdateMember;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class MembersController : ApiController
    {
        [AllowAnonymous]
        [HttpPost("createMember")]
        public async Task<ActionResult> CreateMemberAsync(CreateMemberRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request), "Member Created successfully"));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPut("updateMember")]
        public async Task<IActionResult> UpdateMember(UpdateMemberRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request), "Update Member's Profile Successfully"));
            }
            catch (NotFoundException notFoundException)
            {
                return Ok(new NotFoundError(notFoundException));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }
    }
}