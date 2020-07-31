using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Members.Commands.ActivateMember;
using Fortifex4.Shared.Members.Queries.GetMember;
using Fortifex4.Shared.Members.Queries.Login;
using Fortifex4.Shared.Members.Queries.LoginExternal;
using Fortifex4.Shared.Members.Queries.MemberUsernameAlreadyExists;
using Fortifex4.WebAPI.Common;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        private readonly ICurrentWeb _currentWeb;

        public AccountController(ICurrentWeb currentWeb)
        {
            _currentWeb = currentWeb;
        }

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
        [HttpGet("login-external")]
        public async Task LoginExternal(string scheme)
        {
            AuthenticationProperties authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginExternalCallback", "Account", new { scheme })
            };

            await HttpContext.ChallengeAsync(scheme, authenticationProperties);
        }

        public async Task<IActionResult> LoginExternalCallbackAsync(string scheme)
        {
            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(scheme);

            if (authenticateResult.Succeeded)
            {
                string memberUsername = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
                string externalID = authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string name = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name);

                string pictureUrl = PictureURLHelper.GetPictureURL(authenticateResult);

                LoginExternalRequest request = new LoginExternalRequest
                {
                    MemberUsername = memberUsername,
                    ExternalID = externalID,
                    SchemeProvider = SchemeProvider.FromString(scheme),
                    FullName = name,
                    PictureURL = pictureUrl
                };

                var response = await Mediator.Send(request);

                if (!string.IsNullOrEmpty(response.Token))
                {
                    return Redirect($"{_currentWeb.BaseURL}/account/login-external?token={response.Token}");
                }
                else
                {
                    return Redirect($"{_currentWeb.BaseURL}/account/login");
                }
            }
            else
            {
                return Redirect($"{_currentWeb.BaseURL}/account/login");
            }
        }

        [AllowAnonymous]
        [HttpGet("login-auto")]
        public async Task<IActionResult> LoginAuto()
        {
            LoginRequest request = new LoginRequest
            {
                MemberUsername = "fuady@live.com",
                Password = "111111"
            };

            var response = await Mediator.Send(request);

            return Ok(response);
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