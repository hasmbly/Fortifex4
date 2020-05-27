using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Currencies.Queries.GetDistinctCurrenciesByMemberID
{
    public class CurrencyDTO : IMapFrom<Currency>
    {
        public int CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Currency, CurrencyDTO>();
        }
    }
}