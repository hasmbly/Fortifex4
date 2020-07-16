using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Shared.Members.Commands.ActivateMember;
using Fortifex4.Shared.Members.Queries.GetMember;
using Fortifex4.Shared.Members.Queries.Login;
using Fortifex4.Shared.Members.Queries.MemberUsernameAlreadyExists;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        [AllowAnonymous]
        [HttpGet("checkUsername/{memberUsername}")]
        public async Task<ActionResult> CheckUsername(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new MemberUsernameAlreadyExistsRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
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

        [AllowAnonymous]
        [HttpGet("activate")]
        public async Task<IActionResult> Activate(string code)
        {
            if (string.IsNullOrEmpty(code))
                return Ok($"You didn't provide any Activation Code.");

            try
            {
                Guid activationCode = new Guid(code);

                return Ok(new Success(await Mediator.Send(new ActivateMemberRequest
                {
                    ActivationCode = activationCode
                })));
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

        [Authorize]
        [HttpGet("getMember/{memberUsername}")]
        public async Task<IActionResult> GetMember(string memberUsername)
        {
            try
            {
                GetMemberRequest request = new GetMemberRequest()
                {
                    MemberUsername = memberUsername
                };

                return Ok(new Success(await Mediator.Send(request)));
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