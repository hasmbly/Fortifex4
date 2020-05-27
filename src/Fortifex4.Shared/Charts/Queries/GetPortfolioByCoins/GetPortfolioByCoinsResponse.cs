using System.Collections.Generic;

namespace Fortifex4.Shared.Charts.Queries.GetPortfolioByCoins
{
    public class GetPortfolioByCoinsResponse
    {
        public string MemberUsername { get; set; }
        public string Title { get; set; }
        public string PreferredFiatCurrencySymbol { get; set; }
        public string PreferredCoinCurrencySymbol { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? TotalValueInCrypto { get; set; }

        public IList<string> Labels { get; set; }
        public IList<decimal?> Value { get; set; }
        public IList<decimal?> ValueInCrypto { get; set; }
        public IList<decimal?> ProfitLoss { get; set; }

        public GetPortfolioByCoinsResponse()
        {
            this.Labels = new List<string>();
            this.Value = new List<decimal?>();
            this.ValueInCrypto = new List<decimal?>();
            this.ProfitLoss = new List<decimal?>();
        }
    }
}