using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Domain.Exceptions;
using Fortifex4.Shared.InternalTransfers.Commands.CreateInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Commands.DeleteInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Commands.UpdateInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Queries.GetInternalTransfer;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using Fortifex4.Shared.Wallets.Commands.DeleteWallet;
using Fortifex4.Shared.Wallets.Commands.SyncPersonalWallet;
using Fortifex4.Shared.Wallets.Commands.UpdatePersonalWallet;
using Fortifex4.Shared.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain;
using Fortifex4.Shared.Wallets.Queries.GetPersonalWallets;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Fortifex4.Shared.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class InternalTransfersController : ApiController
    {
        [AllowAnonymous]
        [HttpPost("createInternalTransfer")]
        public async Task<ActionResult> CreateInternalTransfer(CreateInternalTransferRequest request)
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
        [HttpGet("getInternalTransfer/{internalTransferID}")]
        public async Task<IActionResult> GetInternalTransfer(int internalTransferID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetInternalTransferRequest() { InternalTransferID = internalTransferID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPut("updateInternalTransfer")]
        public async Task<IActionResult> UpdateInternalTransfer(UpdateInternalTransferRequest request)
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
        [HttpPost("deleteInternalTransfer")]
        public async Task<IActionResult> DeleteInternalTransfer(DeleteInternalTransferRequest request)
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
        [HttpGet("getWalletsWithSameCurrency/{walletID}")]
        public async Task<IActionResult> GetWalletsWithSameCurrency(int walletID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetWalletsBySameUsernameAndBlockchainRequest() { WalletID = walletID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getAllWalletsWithSameCurrency/{memberUsername}")]
        public async Task<IActionResult> GetAllWalletsWithSameCurrency(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetAllWalletsBySameUsernameAndBlockchainRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}