using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class BlockchainsController : ApiController
    {
        [AllowAnonymous]
        [HttpGet("getAllBlockchains")]
        public async Task<IActionResult> GetAllBlockchains()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetAllBlockchainsRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}