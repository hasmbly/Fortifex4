using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Currencies.Queries.GetPreferrableCoinCurrencies
{
    public class CoinCurrencyDTO : IMapFrom<Currency>
    {
        public int CurrencyID { get; set; }
        public int BlockchainID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public bool IsShownInTradePair { get; set; }
        public bool IsForPreferredOption { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Currency, CoinCurrencyDTO>();
        }

        public string NameWithSymbol
        {
            get
            {
                return $"{this.Name} ({this.Symbol})";
            }
        }
    }
}