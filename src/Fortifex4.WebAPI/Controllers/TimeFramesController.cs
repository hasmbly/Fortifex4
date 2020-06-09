using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.TimeFrames.Queries.GetAllTimeFrames;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class TimeFramesController : ApiController
    {
        [Authorize]
        [HttpGet("getAllTimeFrames")]
        public async Task<IActionResult> GetAllTimeFrames()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetAllTimeFramesRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}