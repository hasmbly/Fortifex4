using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Currencies.Commands.UpdateCryptoCurrencies
{
    public class UpdateCryptoCurrenciesResponse : GeneralResponse
    {
        public IList<BlockchainDTO> Blockchains { get; set; }

        public UpdateCryptoCurrenciesResponse()
        {
            this.Blockchains = new List<BlockchainDTO>();
        }
    }
}