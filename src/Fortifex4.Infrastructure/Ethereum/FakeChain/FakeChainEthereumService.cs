using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Ethereum;
using Fortifex4.Infrastructure.Common;
using Fortifex4.Infrastructure.Constants;

namespace Fortifex4.Infrastructure.Ethereum.FakeChain
{
    public class FakeChainEthereumService : IEthereumService
    {
        public async Task<EthereumWallet> GetEthereumWalletAsync(string address)
        {
            var result = new EthereumWallet();

            //https://fakechain.vioren.com/api/eth/getAddressInfo/0xb297cacf0f91c86dd9d2fb47c6d12783121ab780
            string uri = $"{EthereumServiceProviders.FakeChain.GetAddressInfoEndpointURL}/{address}";

            var walletJSON = await ExternalWebAPIRequestor.GetAsync<WalletJSON>(uri);

            result.Balance = walletJSON.balance;

            foreach (var tokenJSON in walletJSON.tokens)
            {
                result.Tokens.Add(new Token
                {
                    Name = tokenJSON.name,
                    Symbol = tokenJSON.symbol,
                    Address = tokenJSON.address,
                    Balance = tokenJSON.balance
                });
            }

            return result;
        }

        public async Task<EthereumTransactionCollection> GetEthereumTransactionCollectionAsync(string address)
        {
            var result = new EthereumTransactionCollection();

            //https://fakechain.vioren.com/api/eth/getAddressTransactions/0xb297cacf0f91c86dd9d2fb47c6d12783121ab780
            string uri = $"{EthereumServiceProviders.FakeChain.GetAddressTransactionsEndpointURL}/{address}";

            var transactionCollectionJSON = await ExternalWebAPIRequestor.GetAsync<TransactionCollectionJSON>(uri);

            foreach (var transactionJSON in transactionCollectionJSON.transactions)
            {
                result.Transactions.Add(new EthereumTransaction
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