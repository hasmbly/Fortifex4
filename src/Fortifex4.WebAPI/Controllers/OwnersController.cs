using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Lookup.Queries.GetOwners;
using Fortifex4.Shared.Owners.Commands.CreateExchangeOwner;
using Fortifex4.Shared.Owners.Commands.DeleteOwner;
using Fortifex4.Shared.Owners.Commands.UpdateExchangeOwner;
using Fortifex4.Shared.Owners.Queries.GetExchangeOwners;
using Fortifex4.Shared.Owners.Queries.GetOwner;
using Fortifex4.Shared.Providers.Queries.GetAvailableExchangeProviders;
using Fortifex4.Shared.Providers.Queries.GetProvider;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class OwnersController : ApiController
    {
        [Authorize]
        [HttpGet("getOwner/{ownerID}")]
        public async Task<IActionResult> GetOwner(int ownerID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetOwnerRequest() { OwnerID = ownerID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getOwners/{memberUsername}")]
        public async Task<IActionResult> GetOwners(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetOwnersRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getProvider/{providerID}")]
        public async Task<IActionResult> GetProvider(int providerID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetProviderRequest() { ProviderID = providerID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getExchangeOwners/{memberUsername}")]
        public async Task<IActionResult> GetExchangeOwners(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetExchangeOwnersRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getAvailableExchangeProviders/{memberUsername}")]
        public async Task<IActionResult> GetAvailableExchangeProviders(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetAvailableExchangeProvidersRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpPost("createExchangeOwner")]
        public async Task<ActionResult> CreateExchangeOwner(CreateExchangeOwnerRequest request)
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
        [HttpPut("updateExchangeOwner")]
        public async Task<IActionResult> UpdateExchangeOwner(UpdateExchangeOwnerRequest request)
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
        [HttpPost("deleteOwner")]
        public async Task<IActionResult> DeleteOwner(DeleteOwnerRequest request)
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
    }
}