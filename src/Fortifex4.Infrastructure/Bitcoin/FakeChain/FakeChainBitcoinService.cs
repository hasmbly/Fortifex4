using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Bitcoin;
using Fortifex4.Infrastructure.Common;
using Fortifex4.Infrastructure.Constants;

namespace Fortifex4.Infrastructure.Bitcoin.FakeChain
{
    public class FakeChainBitcoinService : IBitcoinService
    {
        public async Task<CryptoWallet> GetBitcoinWalletAsync(string address)
        {
            var result = new CryptoWallet();

            //https://fakechain.vioren.com/api/btc/getAddressInfo/0xb297cacf0f91c86dd9d2fb47c6d12783121ab780
            string uri = $"{BitcoinServiceProviders.FakeChain.GetAddressInfoEndpointURL}/{address}";

            var walletJSON = await ExternalWebAPIRequestor.GetAsync<WalletJSON>(uri);

            result.Balance = walletJSON.balance;

            return result;
        }

        public async Task<BitcoinTransactionCollection> GetBitcoinTransactionCollectionAsync(string address)
        {
            var result = new BitcoinTransactionCollection();

            //https://fakechain.vioren.com/api/btc/getAddressTransactions/0xb297cacf0f91c86dd9d2fb47c6d12783121ab780
            string uri = $"{BitcoinServiceProviders.FakeChain.GetAddressTransactionsEndpointURL}/{address}";

            var transactionCollectionJSON = await ExternalWebAPIRequestor.GetAsync<TransactionCollectionJSON>(uri);

            foreach (var transactionJSON in transactionCollectionJSON.transactions)
            {
                result.Transactions.Add(new BitcoinTransaction
                {
                    FromAddress = transactionJSON.fromAddress,
                    ToAddress = transactionJSON.toAddress,
                    Amount = transactionJSON.amount,
                    Hash = transactionJSON.hash,
                    TimeStamp = transactionJSON.timeStamp
                });
            }

            return result;
        }
    }
}