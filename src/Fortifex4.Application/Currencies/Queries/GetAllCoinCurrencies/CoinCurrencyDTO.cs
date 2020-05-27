using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Currencies.Queries.GetAllCoinCurrencies
{
    public class CoinCurrencyDTO : IMapFrom<Currency>
    {
        public int CurrencyID { get; set; }
        public string BlockchainName { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Currency, CoinCurrencyDTO>()
                .ForMember(dto => dto.BlockchainName, opt => opt.MapFrom(p => p.Blockchain.Name));
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