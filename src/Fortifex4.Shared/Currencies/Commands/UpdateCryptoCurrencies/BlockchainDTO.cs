using System.Collections.Generic;
using Fortifex4.Application.Enums;

namespace Fortifex4.Shared.Currencies.Commands.UpdateCryptoCurrencies
{
    public class BlockchainDTO
    {
        public int BlockchainID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }

        public UpdateStatus UpdateStatus { get; set; }

        public IList<CurrencyDTO> Currencies { get; set; }

        public BlockchainDTO()
        {
            this.Currencies = new List<CurrencyDTO>();
        }
    }
}