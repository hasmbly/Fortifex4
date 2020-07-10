using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Shared.Members.Commands.CreateMember;
using Fortifex4.Shared.Members.Commands.UpdateMember;
using Fortifex4.Shared.Members.Commands.UpdatePreferredCoinCurrency;
using Fortifex4.Shared.Members.Commands.UpdatePreferredFiatCurrency;
using Fortifex4.Shared.Members.Commands.UpdatePreferredTimeFrame;
using Fortifex4.Shared.Members.Queries.GetPreferences;
using Fortifex4.Shared.Transactions.Queries.GetTransactionsByMemberUsername;
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

        [Authorize]
        [HttpPut("updatePreferredTimeFrame")]
        public async Task<IActionResult> UpdatePreferredTimeFrame(UpdatePreferredTimeFrameRequest request)
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

        [Authorize]
        [HttpPut("updatePreferredCoinCurrency")]
        public async Task<IActionResult> UpdatePreferredCoinCurrency(UpdatePreferredCoinCurrencyRequest request)
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

        [Authorize]
        [HttpPut("updatePreferredFiatCurrency")]
        public async Task<IActionResult> UpdatePreferredFiatCurrency(UpdatePreferredFiatCurrencyRequest request)
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

        [Authorize]
        [HttpGet("getPreferences/{memberUsername}")]
        public async Task<IActionResult> GetPreferences(string memberUsername)
        {
            try
            {
                var request = new GetPreferencesRequest()
                {
                    MemberUsername = memberUsername
                };

                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getTransactionsByMemberUsername/{memberUsername}")]
        public async Task<IActionResult> GetTransactionsByMemberUsername(string memberUsername)
        {
            try
            {
                var request = new GetTransactionsByMemberUsernameRequest()
                {
                    MemberUsername = memberUsername
                };

                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}