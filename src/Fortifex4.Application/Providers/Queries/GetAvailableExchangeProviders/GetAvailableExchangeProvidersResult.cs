using System.Collections.Generic;

namespace Fortifex4.Application.Providers.Queries.GetAvailableExchangeProviders
{
    public class GetAvailableExchangeProvidersResult
    {
        public IList<ExchangeProviderDTO> ExchangeProviders { get; set; }

        public GetAvailableExchangeProvidersResult()
        {
            this.ExchangeProviders = new List<ExchangeProviderDTO>();
        }
    }
}