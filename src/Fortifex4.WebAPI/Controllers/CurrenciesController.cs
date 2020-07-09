using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.Currencies.Queries.GetAllCoinCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetAvailableCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetCurrency;
using Fortifex4.Shared.Currencies.Queries.GetDestinationCurrenciesForMember;
using Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI.Controllers
{
    public class CurrenciesController : ApiController
    {
        [Authorize]
        [HttpGet("getAvailableCurrencies/{ownerID}")]
        public async Task<IActionResult> GetAvailableCurrencies(int ownerID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetAvailableCurrenciesRequest() { OwnerID = ownerID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getCurrency/{currencyID}")]
        public async Task<IActionResult> GetCurrency(int currencyID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetCurrencyRequest() { CurrencyID = currencyID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getDestinationCurrenciesForMember/{memberUsername}")]
        public async Task<IActionResult> GetDestinationCurrenciesForMember(string memberUsername)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetDestinationCurrenciesForMemberRequest() { MemberUsername = memberUsername })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getAllCoinCurrencies")]
        public async Task<IActionResult> GetAllCoinCurrencies()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetAllCoinCurrenciesRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

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