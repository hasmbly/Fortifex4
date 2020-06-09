using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class CurrenciesController : ApiController
    {
        [Authorize]
        [HttpGet("getAllFiatCurrencies")]
        public async Task<IActionResult> GetAllFiatCurrencies()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetAllFiatCurrenciesRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getPreferableCoinCurrencies")]
        public async Task<IActionResult> GetPreferableCoinCurrencies()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetPreferableCoinCurrenciesRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}