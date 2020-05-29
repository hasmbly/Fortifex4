using System.Collections.Generic;

namespace Fortifex4.Shared.Currencies.Commands.UpdateCryptoCurrencies
{
    public class UpdateCryptoCurrenciesResponse
    {
        public IList<BlockchainDTO> Blockchains { get; set; }

        public UpdateCryptoCurrenciesResponse()
        {
            this.Blockchains = new List<BlockchainDTO>();
        }
    }
}