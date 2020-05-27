using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Providers.Queries.GetProvider
{
    public class GetProviderResult
    {
        public int ProviderID { get; set; }
        public string Name { get; set; }
        public ProviderType ProviderType { get; set; }
        public string SiteURL { get; set; }
    }
}