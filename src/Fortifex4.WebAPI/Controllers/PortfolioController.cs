using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Members.Queries.GetPortfolio;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class PortfolioController : ApiController
    {
        [Authorize]
        [HttpGet("getPortfolio/{memberUsername}")]
        public async Task<IActionResult> GetPortfolio(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetPortfolioRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}