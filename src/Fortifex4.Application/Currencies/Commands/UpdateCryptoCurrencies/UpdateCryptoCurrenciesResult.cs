using System.Collections.Generic;

namespace Fortifex4.Application.Currencies.Commands.UpdateCryptoCurrencies
{
    public class UpdateCryptoCurrenciesResult
    {
        public IList<BlockchainDTO> Blockchains { get; set; }

        public UpdateCryptoCurrenciesResult()
        {
            this.Blockchains = new List<BlockchainDTO>();
        }
    }
}