using System;
using System.Threading.Tasks;
using Fortifex4.Shared.Currencies.Queries.GetPriceConversion;
using Fortifex4.Shared.Currencies.Queries.GetUnitPrice;
using Fortifex4.Shared.Currencies.Queries.GetUnitPriceInUSD;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Fortifex4.WebAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fortifex4.WebAPI
{
    public class ToolsController : ApiController
    {
        [AllowAnonymous]
        [HttpGet("getPriceConversion")]
        public async Task<IActionResult> GetPriceConversion(string fromCurrencySymbol, string toCurrencySymbol, decimal amount)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(await Mediator.Send(new GetPriceConversionRequest { FromCurrencySymbol = fromCurrencySymbol, ToCurrencySymbol = toCurrencySymbol, Amount = amount }))));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpGet("getUnitPrice")]
        public async Task<IActionResult> GetUnitPrice(string fromCurrencySymbol, string toCurrencySymbol)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetUnitPriceRequest { FromCurrencySymbol = fromCurrencySymbol, ToCurrencySymbol = toCurrencySymbol })));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpGet("getUnitPriceInUSD")]
        public async Task<IActionResult> GetUnitPriceInUSD(string currencySymbol)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetUnitPriceInUSDRequest { CurrencySymbol = currencySymbol })));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }
    }
}