using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Regions.Queries.GetRegions;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class RegionsController : ApiController
    {
        [Authorize]
        [HttpGet("getRegions/{countryCode}")]
        public async Task<IActionResult> GetRegions(string countryCode)
        {
            try
            {
                var request = new GetRegionsRequest()
                {
                    CountryCode = countryCode
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