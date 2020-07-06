using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Trades.Commands.CreateTrade;
using Fortifex4.Shared.Trades.Commands.DeleteTrade;
using Fortifex4.Shared.Trades.Commands.UpdateTrade;
using Fortifex4.Shared.Trades.Queries.GetTrade;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class TradesController : ApiController
    {
        [AllowAnonymous]
        [HttpPost("createTrade")]
        public async Task<ActionResult> CreateTrade(CreateTradeRequest request)
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
        [HttpGet("getTrade/{tradeID}")]
        public async Task<IActionResult> GetTrade(int tradeID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetTradeRequest() { TradeID = tradeID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPut("updateTrade")]
        public async Task<IActionResult> UpdateTrade(UpdateTradeRequest request)
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
        [HttpPost("deleteTrade")]
        public async Task<IActionResult> DeleteTrade(DeleteTradeRequest request)
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