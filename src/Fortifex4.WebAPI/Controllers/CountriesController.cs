using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Countries.Queries.GetAllCountries;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class CountriesController : ApiController
    {
        [Authorize]
        [HttpGet("getAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetAllCountriesRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}