using System;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Bitcoin;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Exceptions;
using Fortifex4.Infrastructure.Common;
using Fortifex4.Infrastructure.Constants;
using Microsoft.Extensions.Logging;
namespace Fortifex4.Infrastructure.Bitcoin.BlockChain
{
    public class BlockchainBitcoinService : IBitcoinService
    {
        private readonly ILogger<BlockchainBitcoinService> _logger;

        public BlockchainBitcoinService(ILogger<BlockchainBitcoinService> logger)
        {
            _logger = logger;
        }

        public Task<BitcoinTransactionCollection> GetBitcoinTransactionCollectionAsync(string address)
        {
            throw new NotImplementedException();
        }

        public async Task<CryptoWallet> GetBitcoinWalletAsync(string address)
        {
            var result = new CryptoWallet();

            //https://blockchain.info/q/addressbalance/1FKtFQ7Ti9vo7W3hskxZ1nXJpJtSixBdJc
            string uri = $"{BitcoinServiceProviders.Blockchain.AddressBalanceEndpointURL}/{address}";

            _logger.LogDebug($"{nameof(GetBitcoinWalletAsync)}");
            _logger.LogDebug(uri);

            try
            {
                var listWalletJSON = await ExternalWebAPIRequestor.GetAsync(uri);

                result.Balance = Convert.ToDecimal(listWalletJSON) / 100000000;
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Conflict)
            {
                throw new InvalidWalletAddressException(address, CurrencySymbol.BTC);
            }

            return result;
        }
    }
}