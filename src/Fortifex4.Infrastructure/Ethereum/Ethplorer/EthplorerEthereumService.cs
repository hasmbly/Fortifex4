using System.Collections.Generic;
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

namespace Fortifex4.Infrastructure.Ethereum.Ethplorer
{
    public class EthplorerEthereumService : IEthereumService
    {
        private readonly ILogger<EthplorerEthereumService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string APIKey;

        public EthplorerEthereumService(ILogger<EthplorerEthereumService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            APIKey = _configuration[ConfigurationKey.Ethereum.Ethplorer.APIKey];
        }

        public async Task<CryptoWallet> GetEthereumWalletAsync(string address)
        {
            var result = new CryptoWallet();

            //https://api.ethplorer.io/getAddressInfo/0xb297cacf0f91c86dd9d2fb47c6d12783121ab780?apiKey=freekey
            string uri = $"{EthereumServiceProviders.Ethplorer.GetAddressInfoEndpointURL}/{address}?apiKey={APIKey}";

            _logger.LogDebug($"{nameof(GetEthereumWalletAsync)}");
            _logger.LogDebug(uri);

            try
            {
                var walletJSON = await ExternalWebAPIRequestor.GetAsync<WalletJSON>(uri);

                result.Balance = walletJSON.ETH.balance;

                foreach (var tokenJSON in walletJSON.tokens)
                {
                    result.Pockets.Add(new CryptoPocket
                    {
                        Name = tokenJSON.tokenInfo.name,
                        Symbol = tokenJSON.tokenInfo.symbol,
                        Address = tokenJSON.tokenInfo.address,
                        Balance = tokenJSON.CalculatedBalance
                    });
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotAcceptable)
            {
                throw new InvalidWalletAddressException(address, CurrencySymbol.ETH);
            }

            return result;
        }

        public async Task<EthereumTransactionCollection> GetEthereumTransactionCollectionAsync(string address)
        {
            var result = new EthereumTransactionCollection();

            //https://api.ethplorer.io/getAddressTransactions/0xb297cacf0f91c86dd9d2fb47c6d12783121ab780?apiKey=freekey
            string uri = $"{EthereumServiceProviders.Ethplorer.GetAddressTransactionsEndpointURL}/{address}?apiKey={APIKey}";

            _logger.LogDebug($"{nameof(GetEthereumTransactionCollectionAsync)}");
            _logger.LogDebug(uri);

            try
            {
                var listTransactionJSON = await ExternalWebAPIRequestor.GetAsync<List<TransactionJSON>>(uri);

                foreach (var transactionJSON in listTransactionJSON)
                {
                    result.Transactions.Add(new EthereumTransaction
                    {
                        FromAddress = transactionJSON.from,
                        ToAddress = transactionJSON.to,
                        Amount = transactionJSON.value,
                        Hash = transactionJSON.hash,
                        TimeStamp = transactionJSON.timestamp
                    });
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotAcceptable)
            {
                throw new InvalidWalletAddressException(address, CurrencySymbol.ETH);
            }

            return result;
        }
    }
}