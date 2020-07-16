using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fortifex4.Shared;
using Fortifex4.Shared.Currencies.Commands.UpdateCryptoCurrencies;
using Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencies;
using Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencyCoinMarketCapIDs;
using Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencyNames;
using Fortifex4.Shared.System.Commands.RemoveAllTransactions;
using Fortifex4.Shared.System.Commands.SeedMasterData;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fortifex4.WebAPI.Controllers
{
    public class DevController : ApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public DevController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ASPNETCORE_ENVIRONMENT: {_webHostEnvironment.EnvironmentName}");
            sb.AppendLine($"AppContext.BaseDirectory: {AppContext.BaseDirectory}");
            sb.AppendLine($"ConnectionString FortifexDatabase: {_configuration.GetSection("ConnectionStrings")["FortifexDatabase"]}");

            return Content(sb.ToString());
        }

        [AllowAnonymous]
        [HttpGet("getFortifexOption/{subSection}")]
        public ActionResult GetFortifexOption(string subSection)
        {
            try
            {
                return Ok(new Success(_configuration.GetSection(FortifexOptions.RootSection)[subSection]));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpGet("seedMasterData")]
        public async Task<ActionResult> SeedMasterDataAsync()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new SeedMasterDataRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpGet("seedFiatCurrencies")]
        public async Task<ActionResult> SeedFiatCurrenciesAsync()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new UpdateFiatCurrenciesRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpGet("seedCryptoCurrencies")]
        public async Task<ActionResult> SeedCryptoCurrenciesAsync()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new UpdateCryptoCurrenciesRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpGet("removeAllTransactions")]
        public async Task<ActionResult> RemoveAllTransactionsAsync()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new RemoveAllTransactionsRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpGet("updateFiatCurrencyNames")]
        public async Task<ActionResult> UpdateFiatCurrencyNamesAsync()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new UpdateFiatCurrencyNamesRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [AllowAnonymous]
        [HttpGet("updateFiatCurrencyCoinMarketCapIDs")]
        public async Task<ActionResult> UpdateFiatCurrencyCoinMarketCapIDsAsync()
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new UpdateFiatCurrencyCoinMarketCapIDsRequest())));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }
    }
}