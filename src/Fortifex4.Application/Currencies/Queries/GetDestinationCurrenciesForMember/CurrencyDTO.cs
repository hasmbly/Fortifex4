using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class CurrencyDTO : IMapFrom<Currency>
    {
        public int CurrencyID { get; set; }
        public string BlockchainName { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Currency, CurrencyDTO>()
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