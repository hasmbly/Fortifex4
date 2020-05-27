using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Providers.Queries.GetAvailableExchangeProviders
{
    public class ExchangeProviderDTO
    {
        public int ProviderID { get; set; }
        public string Name { get; set; }
        public string SiteURL { get; set; }
    }
}