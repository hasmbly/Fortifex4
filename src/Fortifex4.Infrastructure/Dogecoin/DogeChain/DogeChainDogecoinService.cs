using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Dogecoin;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Exceptions;
using Fortifex4.Infrastructure.Common;
using Fortifex4.Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Infrastructure.Dogecoin.DogeChain
{
    public class DogeChainDogecoinService : IDogecoinService
    {
        private readonly ILogger<DogeChainDogecoinService> _logger;

        public DogeChainDogecoinService(ILogger<DogeChainDogecoinService> logger)
        {
            _logger = logger;
        }

        public async Task<CryptoWallet> GetDogecoinWalletAsync(string address)
        {
            var response = new CryptoWallet();

            //https://dogechain.info/api/v1/address/balance/DBXu2kgc3xtvCUWFcxFE3r9hEYgmuaaCyD
            string uri = $"{DogecoinServiceProviders.DogeChain.BalanceEndpointURL}/{address}";

            _logger.LogDebug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {uri}");

            try
            {
                var walletJSON = await ExternalWebAPIRequestor.GetAsync<WalletJSON>(uri);

                if (walletJSON.success == 1)
                {
                    response.Balance = walletJSON.balance;
                }
                else
                {
                    throw new InvalidOperationException(walletJSON.error);
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                throw new InvalidWalletAddressException(address, CurrencySymbol.DOGE);
            }

            return response;
        }

        public Task<DogecoinTransactionCollection> GetDogecoinTransactionCollectionAsync(string address)
        {
            throw new NotImplementedException();
        }
    }
}