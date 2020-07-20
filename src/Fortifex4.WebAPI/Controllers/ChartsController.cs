using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Charts.Queries.GetCoinByExchanges;
using Fortifex4.Shared.Charts.Queries.GetPortfolioByCoinsV2;
using Fortifex4.Shared.Charts.Queries.GetPortfolioByExchanges;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class ChartsController : ApiController
    {
        [Authorize]
        [HttpGet("getPortfolioByCoinsV2/{memberUsername}")]
        public async Task<IActionResult> GetPortfolioByCoinsV2(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetPortfolioByCoinsV2Request() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getPortfolioByExchanges/{memberUsername}")]
        public async Task<IActionResult> GetPortfolioByExchanges(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetPortfolioByExchangesRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getCoinByExchanges/{memberUsername}/{currencyID}")]
        public async Task<IActionResult> GetCoinByExchanges(string memberUsername, int currencyID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetCoinByExchangesRequest() { MemberUsername = memberUsername, CurrencyID = currencyID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}