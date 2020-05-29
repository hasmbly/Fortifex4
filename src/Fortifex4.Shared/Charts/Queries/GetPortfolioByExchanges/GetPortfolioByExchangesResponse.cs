using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Charts.Queries.GetPortfolioByExchanges
{
    public class GetPortfolioByExchangesResponse : GeneralResponse
    {
        public string MemberUsername { get; set; }
        public string Title { get; set; }
        public string FiatCode { get; set; }
        public string CryptoCode { get; set; }

        public decimal? TotalValue { get; set; }
        public decimal? TotalValueInCrypto { get; set; }
        public decimal? TotalProfitLoss { get; set; }
        public decimal? TotalProfitLossInCrypto { get; set; }

        public IList<string> Labels { get; set; }
        public IList<decimal?> Value { get; set; }

        public IList<decimal?> ProfitLoss { get; set; }
        public IList<decimal?> ProfitLossInCrypto { get; set; }
        public IList<decimal?> ValueInCrypto { get; set; }

        public GetPortfolioByExchangesResponse()
        {
            this.Labels = new List<string>();
            this.Value = new List<decimal?>();

            this.ProfitLoss = new List<decimal?>();
            this.ProfitLossInCrypto = new List<decimal?>();
            this.ValueInCrypto = new List<decimal?>();
        }
    }
}