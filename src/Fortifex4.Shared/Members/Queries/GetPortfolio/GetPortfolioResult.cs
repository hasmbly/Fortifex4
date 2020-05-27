using System;
using System.Collections.Generic;
using System.Linq;

namespace Fortifex4.Application.Members.Queries.GetPortfolio
{
    public class GetPortfolioResult
    {
        public int MemberPreferredFiatCurrencyID { get; set; }
        public string MemberPreferredFiatCurrencySymbol { get; set; }
        public decimal MemberPreferredFiatCurrencyUnitPriceInUSD { get; set; }
        public int MemberPreferredCoinCurrencyID { get; set; }
        public string MemberPreferredCoinCurrencySymbol { get; set; }
        public decimal MemberPreferredCoinCurrencyUnitPriceInUSD { get; set; }
        public string MemberPreferredTimeFrameName { get; set; }

        public IList<CurrencyDTO> Currencies { get; set; }

        public GetPortfolioResult()
        {
            this.Currencies = new List<CurrencyDTO>();
        }

        public decimal TotalPurchaseValueInPreferredFiatCurrency => this.Currencies.Sum(x => x.TotalPurchaseValueInPreferredFiatCurrency);

        public decimal TotalCurrentValueInPreferredFiatCurrency => this.Currencies.Sum(x => x.CurrentValueInPreferredFiatCurrency);

        public decimal TotalCurrentValueInPreferredCoinCurrency => this.Currencies.Sum(x => x.CurrentValueInPreferredCoinCurrency);

        public decimal OneDayChangeValueInPreferredFiatCurrency => this.Currencies.Sum(x => x.ValueChange24hInPreferredFiatCurrency);

        public decimal LifeTimeChangeValueInPreferredFiatCurrency => this.Currencies.Sum(x => x.ProfitLossInPreferredFiatCurrency);

        public float LifeTimeChangeRateInPreferredFiatCurrency
        {
            get
            {
                if (this.TotalPurchaseValueInPreferredFiatCurrency > 0)
                {
                    return Convert.ToSingle(this.LifeTimeChangeValueInPreferredFiatCurrency / this.TotalPurchaseValueInPreferredFiatCurrency);
                }
                else
                {
                    return 0f;
                }
            }
        }

        public decimal TotalYesterdayValueInPreferredFiatCurrency => this.Currencies.Sum(x => x.YesterdayValueInPreferredFiatCurrency);
        public decimal TotalValueChange24hInPreferredFiatCurrency => this.TotalCurrentValueInPreferredFiatCurrency - this.TotalYesterdayValueInPreferredFiatCurrency;

        public float OneDayChangeRateInPreferredFiatCurrency
        {
            get
            {
                if (this.TotalYesterdayValueInPreferredFiatCurrency > 0)
                {
                    return Convert.ToSingle(this.TotalValueChange24hInPreferredFiatCurrency / this.TotalYesterdayValueInPreferredFiatCurrency);
                }
                else
                {
                    return 0f;
                }
            }
        }
    }
}