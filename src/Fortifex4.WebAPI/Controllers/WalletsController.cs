using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Domain.Exceptions;
using Fortifex4.Shared.Pockets.Queries.GetPocket;
using Fortifex4.Shared.Sync.Commands.UpdateSync;
using Fortifex4.Shared.Sync.Queries.GetSync;
using Fortifex4.Shared.Wallets.Commands.CreateExchangeWallet;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using Fortifex4.Shared.Wallets.Commands.DeleteWallet;
using Fortifex4.Shared.Wallets.Commands.SyncPersonalWallet;
using Fortifex4.Shared.Wallets.Commands.UpdatePersonalWallet;
using Fortifex4.Shared.Wallets.Queries.GetPersonalWallets;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Fortifex4.WebAPI.Controllers
{
    public class WalletsController : ApiController
    {
        [Authorize]
        [HttpGet("getPersonalWallets/{memberUsername}")]
        public async Task<IActionResult> GetPersonalWallets(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetPersonalWalletsRequest() { MemberUsername = memberUsername } )));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getWallet/{walletID}")]
        public async Task<IActionResult> GetWallet(int walletID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetWalletRequest() { WalletID = walletID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getPocket/{pocketID}")]
        public async Task<IActionResult> GetPocket(int pocketID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetPocketRequest() { PocketID = pocketID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpPost("createPersonalWallet")]
        public async Task<ActionResult> CreatePersonalWallet(CreatePersonalWalletRequest request)
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

        [AllowAnonymous]
        [HttpPost("createExchangeWallet")]
        public async Task<ActionResult> CreateExchangeWallet(CreateExchangeWalletRequest request)
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
        [HttpPut("updatePersonalWallet")]
        public async Task<IActionResult> UpdatePersonalWallet(UpdatePersonalWalletRequest request)
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
        [HttpPost("deleteWallet")]
        public async Task<IActionResult> DeleteWallet(DeleteWalletRequest request)
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
        [HttpGet("syncPersonalWallet/{walletID}")]
        public async Task<IActionResult> SyncPersonalWallet(int walletID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new SyncPersonalWalletRequest() { WalletID = walletID })));
            }
            catch (InvalidWalletAddressException iwaex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(iwaex.Message));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("sync/details/{transactionID}")]
        public async Task<IActionResult> GetSyncPersonalWallet(int transactionID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetSyncRequest() { TransactionID = transactionID })));
            }
            catch (InvalidWalletAddressException iwaex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(iwaex.Message));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPut("sync/edit")]
        public async Task<IActionResult> UpdateSyncPersonalWallet(UpdateSyncRequest request)
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