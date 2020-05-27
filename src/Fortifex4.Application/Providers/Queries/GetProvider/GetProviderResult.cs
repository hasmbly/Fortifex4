using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Providers.Queries.GetProvider
{
    public class GetProviderResult : IMapFrom<Provider>
    {
        public int ProviderID { get; set; }
        public string Name { get; set; }
        public ProviderType ProviderType { get; set; }
        public string SiteURL { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Provider, GetProviderResult>();
        }
    }
}