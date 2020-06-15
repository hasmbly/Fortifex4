using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Bitcoin;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Exceptions;
using Fortifex4.Infrastructure.Common;
using Fortifex4.Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Infrastructure.Bitcoin.BitcoinChain
{
    public class BitcoinChainBitcoinService : IBitcoinService
    {
        private readonly ILogger<BitcoinChainBitcoinService> _logger;

        public BitcoinChainBitcoinService(ILogger<BitcoinChainBitcoinService> logger)
        {
            _logger = logger;
        }

        public async Task<CryptoWallet> GetBitcoinWalletAsync(string address)
        {
            var result = new CryptoWallet();

            //https://api-r.bitcoinchain.com/v1/address/1Chain4asCYNnLVbvG6pgCLGBrtzh4Lx4b
            string uri = $"{BitcoinServiceProviders.BitcoinChain.AddressEndpointURL}/{address}";

            _logger.LogDebug($"{nameof(GetBitcoinWalletAsync)}");
            _logger.LogDebug(uri);

            try
            {
                var listWalletJSON = await ExternalWebAPIRequestor.GetAsync<List<WalletJSON>>(uri);

                var walletJSON = listWalletJSON.FirstOrDefault();

                result.Balance = walletJSON.balance;
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Conflict)
            {
                throw new InvalidWalletAddressException(address, CurrencySymbol.BTC);
            }

            return result;
        }

        public async Task<BitcoinTransactionCollection> GetBitcoinTransactionCollectionAsync(string address)
        {
            var result = new BitcoinTransactionCollection();

            //https://api-r.bitcoinchain.com/v1/address/txs/1Chain4asCYNnLVbvG6pgCLGBrtzh4Lx4b
            string uri = $"{BitcoinServiceProviders.BitcoinChain.AddressTxsEndpointURL}/{address}";

            _logger.LogDebug($"{nameof(GetBitcoinTransactionCollectionAsync)}");
            _logger.LogDebug(uri);

            try
            {
                var listListTransactionContainerJSON = await ExternalWebAPIRequestor.GetAsync<List<List<TransactionContainerJSON>>>(uri);

                var firstListTransactionContainerJSON = listListTransactionContainerJSON.FirstOrDefault();

                if (firstListTransactionContainerJSON != null)
                {
                    foreach (var transactionContainer in firstListTransactionContainerJSON)
                    {
                        result.Transactions.Add(new BitcoinTransaction
                        {
                            FromAddress = transactionContainer.tx.inputs[0].sender,
                            ToAddress = transactionContainer.tx.outputs[0].receiver,
                            Amount = transactionContainer.tx.total_output,
                            Hash = transactionContainer.tx.self_hash,
                            TimeStamp = transactionContainer.tx.block_time
                        });
                    }
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Conflict)
            {
                throw new InvalidWalletAddressException(address, CurrencySymbol.BTC);
            }

            return result;
        }
    }
}