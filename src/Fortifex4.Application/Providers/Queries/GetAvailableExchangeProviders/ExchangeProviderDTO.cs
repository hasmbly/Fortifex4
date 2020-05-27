using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Providers.Queries.GetAvailableExchangeProviders
{
    public class ExchangeProviderDTO : IMapFrom<Provider>
    {
        public int ProviderID { get; set; }
        public string Name { get; set; }
        public string SiteURL { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Provider, ExchangeProviderDTO>();
        }
    }
}