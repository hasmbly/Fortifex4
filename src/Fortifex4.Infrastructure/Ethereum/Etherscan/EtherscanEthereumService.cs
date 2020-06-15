using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Ethereum;
using Fortifex4.Domain.Constants;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Exceptions;
using Fortifex4.Infrastructure.Common;
using Fortifex4.Infrastructure.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Infrastructure.Ethereum.Etherscan
{
    public class EtherscanEthereumService : IEthereumService
    {
        private readonly ILogger<EtherscanEthereumService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string APIKey;

        public EtherscanEthereumService(ILogger<EtherscanEthereumService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            APIKey = _configuration[ConfigurationKey.Ethereum.Etherscan.APIKey];
        }

        public async Task<CryptoWallet> GetEthereumWalletAsync(string address)
        {
            var result = new CryptoWallet();

            //https://api.etherscan.io/api?module=account&action=balance&address=0xddbd2b932c763ba5b1b7ae3b362eac3e8d40121a&tag=latest&apikey=CVMMDNQRGURVWCGMBUTEF14V4VM1K147VP
            string uri = $"{EthereumServiceProviders.Etherscan.AccountBalanceEndpointURL}&address={address}&apikey={APIKey}";

            _logger.LogDebug($"{nameof(GetEthereumWalletAsync)}");
            _logger.LogDebug(uri);

            try
            {
                var walletJSON = await ExternalWebAPIRequestor.GetAsync<WalletJSON>(uri);

                result.Balance = Convert.ToDecimal(walletJSON.result) * Convert.ToDecimal(Math.Pow(10, -18));
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotAcceptable)
            {
                throw new InvalidWalletAddressException(address, CurrencySymbol.ETH);
            }

            return result;
        }

        public Task<EthereumTransactionCollection> GetEthereumTransactionCollectionAsync(string address)
        {
            throw new NotImplementedException();
        }
    }
}