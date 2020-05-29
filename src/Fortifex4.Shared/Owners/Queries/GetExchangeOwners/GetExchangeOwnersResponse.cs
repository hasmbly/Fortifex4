using System.Collections.Generic;
using Fortifex4.Shared.Owners.Common;

namespace Fortifex4.Shared.Owners.Queries.GetExchangeOwners
{
    public class GetExchangeOwnersResponse : WalletContainer
    {
        public IList<ExchangeOwnerDTO> ExchangeOwners { get; set; }

        public GetExchangeOwnersResponse()
        {
            this.ExchangeOwners = new List<ExchangeOwnerDTO>();
        }
    }
}