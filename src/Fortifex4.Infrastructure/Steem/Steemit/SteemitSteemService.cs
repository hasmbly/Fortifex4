using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Steem;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Exceptions;
using Fortifex4.Infrastructure.Constants;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fortifex4.Infrastructure.Steem.Steemit
{
    public class SteemitSteemService : ISteemService
    {
        private readonly ILogger<SteemitSteemService> _logger;
        private static readonly HttpClient client = new HttpClient();

        public SteemitSteemService(ILogger<SteemitSteemService> logger)
        {
            _logger = logger;
        }

        public async Task<CryptoWallet> GetSteemWalletAsync(string address)
        {
            var result = new CryptoWallet();

            //https://api.openhive.network
            string uri = $"{SteemServiceProviders.Steemit.BaseAPIEndpointURL}";

            _logger.LogDebug($"{nameof(GetSteemWalletAsync)}");
            _logger.LogDebug(uri);

            try
            {
                var getAccountsRequest = new GetAccountsRequestJSON(address);
                var getAccountsRequestJSON = JsonConvert.SerializeObject(getAccountsRequest);
                var getAccountsRequestContent = new StringContent(getAccountsRequestJSON, Encoding.UTF8, "application/json");

                var getAccountsResponseMessage = await client.PostAsync(uri, getAccountsRequestContent);
                var getAccountsResponseString = await getAccountsResponseMessage.Content.ReadAsStringAsync();

                var getAccountsResponseJSON = JsonConvert.DeserializeObject<GetAccountsResponseJSON>(getAccountsResponseString);

                var responseResult = getAccountsResponseJSON.result.FirstOrDefault();

                if (responseResult != null)
                {
                    var balanceText = responseResult.balance.Replace($" {CurrencySymbol.STEEM}", string.Empty);
                    result.Balance = Convert.ToDecimal(balanceText);
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Conflict)
            {
                throw new InvalidWalletAddressException(address, CurrencySymbol.STEEM);
            }
            catch
            {
                throw new InvalidWalletAddressException(address, CurrencySymbol.STEEM);
            }

            return result;
        }
    }
}