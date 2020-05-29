using System.Collections.Generic;

namespace Fortifex4.Shared.Providers.Queries.GetAvailableExchangeProviders
{
    public class GetAvailableExchangeProvidersResponse
    {
        public IList<ExchangeProviderDTO> ExchangeProviders { get; set; }

        public GetAvailableExchangeProvidersResponse()
        {
            this.ExchangeProviders = new List<ExchangeProviderDTO>();
        }
    }
}