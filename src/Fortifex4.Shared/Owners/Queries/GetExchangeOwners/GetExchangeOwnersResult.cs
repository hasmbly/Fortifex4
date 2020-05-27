using System.Collections.Generic;
using Fortifex4.Application.Owners.Common;

namespace Fortifex4.Application.Owners.Queries.GetExchangeOwners
{
    public class GetExchangeOwnersResult : WalletContainer
    {
        public IList<ExchangeOwnerDTO> ExchangeOwners { get; set; }

        public GetExchangeOwnersResult()
        {
            this.ExchangeOwners = new List<ExchangeOwnerDTO>();
        }
    }
}