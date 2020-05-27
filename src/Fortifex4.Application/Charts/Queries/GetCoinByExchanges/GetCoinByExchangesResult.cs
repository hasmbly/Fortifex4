using System.Collections.Generic;

namespace Fortifex4.Application.Charts.Queries.GetCoinByExchanges
{
    public class GetCoinByExchangesResult
    {
        public string MemberUsername { get; set; }
        public int CryptocurrencyID { get; set; }
        public string CryptocurrencyName { get; set; }
        public string FiatCode { get; set; }
        public string CryptoCode { get; set; }

        public decimal? TotalValue { get; set; }
        public decimal? TotalValueInCrypto { get; set; }
        
        public IList<string> Labels { get; set; }
        public IList<decimal?> Value { get; set; }
        public IList<decimal?> ValueInCrypto { get; set; }

        public GetCoinByExchangesResult()
        {
            this.Labels = new List<string>();
            this.Value = new List<decimal?>();
            this.ValueInCrypto = new List<decimal?>();
        }
    }
}