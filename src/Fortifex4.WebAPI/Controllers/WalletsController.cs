using System;
using System.Net;
using System.Threading.Tasks;
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
        [HttpGet("syncPersonalWallet/{walletID}")]
        public async Task<IActionResult> SyncPersonalWallet(int walletID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new SyncPersonalWalletRequest() { WalletID = walletID })));
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
    }
}