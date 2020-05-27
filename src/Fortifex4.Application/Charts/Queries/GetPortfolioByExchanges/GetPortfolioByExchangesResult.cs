using System;
using System.Collections.Generic;
using System.Text;

namespace Fortifex4.Application.Charts.Queries.GetPortfolioByExchanges
{
    public class GetPortfolioByExchangesResult
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

        public GetPortfolioByExchangesResult()
        {
            this.Labels = new List<string>();
            this.Value = new List<decimal?>();

            this.ProfitLoss = new List<decimal?>();
            this.ProfitLossInCrypto = new List<decimal?>();
            this.ValueInCrypto = new List<decimal?>();
        }
    }
}