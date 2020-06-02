using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Members.Commands.CreateMember;
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
    }
}