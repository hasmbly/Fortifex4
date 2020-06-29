using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.ExternalTransfers.Commands.UpdateExternalTransfer;
using Fortifex4.Shared.ExternalTransfers.Queries.GetExternalTransfer;
using Fortifex4.Shared.Wallets.Commands.CreateExternalTransfer;
using Fortifex4.Shared.Wallets.Commands.DeleteExternalTransfer;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class ExternalTransfersController : ApiController
    {
        [AllowAnonymous]
        [HttpPost("createExternalTransfer")]
        public async Task<ActionResult> CreateExternalTransfer(CreateExternalTransferRequest request)
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
        [HttpGet("getExternalTransfer/{transactionID}")]
        public async Task<IActionResult> GetExternalTransfer(int transactionID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetExternalTransferRequest() { TransactionID = transactionID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPut("updateExternalTransfer")]
        public async Task<IActionResult> UpdateExternalTransfer(UpdateExternalTransferRequest request)
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
        [HttpPost("deleteExternalTransfer")]
        public async Task<IActionResult> DeleteExternalTransfer(DeleteExternalTransferRequest request)
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