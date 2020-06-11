using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Genders.Queries.GetAllGenders;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class GendersController : ApiController
    {
        [Authorize]
        [HttpGet("getAllGenders")]
        public async Task<IActionResult> GetAllGenders()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetAllGendersRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}