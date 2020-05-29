using System;
using System.Collections.Generic;
using System.Linq;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Shared.Members.Queries.GetPortfolio
{
    public class CurrencyDTO
    {
        public int CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public decimal Price { get; set; }
        public decimal Volume24h { get; set; }
        public float PercentChange1h { get; set; }
        public float PercentChange24h { get; set; }
        public float PercentChange7d { get; set; }
        public float PercentChangeLifetime { get; set; }

        public GetPortfolioResponse Portfolio { get; set; }

        public IList<TransactionDTO> Transactions { get; set; }

        public CurrencyDTO()
        {
            this.Transactions = new List<TransactionDTO>();
        }

        public IList<TransactionDTO> IncomingTransactions
        {
            get
            {
                return this.Transactions
                    .Where(x =>
                        x.TransactionType == TransactionType.StartingBalance ||
                        x.TransactionType == TransactionType.ExternalTransferIN ||
                        x.TransactionType == TransactionType.BuyIN ||
                        x.TransactionType == TransactionType.SellIN ||
                        x.TransactionType == TransactionType.SyncBalanceImport ||
                        x.TransactionType == TransactionType.SyncTransactionIN)
                    .OrderBy(x => x.TransactionDateTime)
                    .ToList();
            }
        }

        public decimal TotalAmount => this.Transactions.Sum(x => x.Amount);

        public decimal TotalPurchaseValueInUSD => this.IncomingTransactions.Sum(x => x.Amount * x.UnitPriceInUSD);

        public decimal TotalPurchaseValueInPreferredFiatCurrency
        {
            get
            {
                if (this.Portfolio.MemberPreferredFiatCurrencyUnitPriceInUSD > 0)
                    return this.TotalPurchaseValueInUSD / this.Portfolio.MemberPreferredFiatCurrencyUnitPriceInUSD;
                else
                    return 0m;
            }
        }

        public decimal AverageBuyPriceInPreferredFiatCurrency
        {
            get
            {
                if (this.TotalAmount > 0)
                    return this.TotalPurchaseValueInPreferredFiatCurrency / this.TotalAmount;
                else
                    return 0m;
            }
        }

        public decimal CurrentValueInUSD => this.TotalAmount * this.UnitPriceInUSD;

        public decimal CurrentValueInPreferredFiatCurrency => this.TotalAmount * this.Price;

        public decimal CurrentValueInPreferredCoinCurrency
        {
            get
            {
                if (this.CurrencyID == this.Portfolio.MemberPreferredCoinCurrencyID)
                {
                    return this.TotalAmount;
                }
                else
                {
                    if (this.Portfolio.MemberPreferredCoinCurrencyUnitPriceInUSD > 0)
                        return this.TotalAmount * (this.UnitPriceInUSD / this.Portfolio.MemberPreferredCoinCurrencyUnitPriceInUSD);
                    else
                        return 0m;
                }
            }
        }

        public float SelectedPercentChange
        {
            get
            {
                return this.Portfolio.MemberPreferredTimeFrameName switch
                {
                    TimeFrameName.OneHour => this.PercentChange1h,
                    TimeFrameName.OneDay => this.PercentChange24h,
                    TimeFrameName.OneWeek => this.PercentChange7d,
                    _ => this.PercentChangeLifetime
                };
            }
        }

        public decimal ProfitLossInPreferredFiatCurrency => this.CurrentValueInPreferredFiatCurrency - this.TotalPurchaseValueInPreferredFiatCurrency;

        public TimeSpan TimeHeld
        {
            get
            {
                if (this.IncomingTransactions.Count > 0)
                {
                    return DateTime.Now - this.IncomingTransactions.First().TransactionDateTime;
                }
                else
                {
                    return new TimeSpan();
                }
            }
        }

        public decimal YesterdayPriceInPreferredFiatCurrency => this.Price / Convert.ToDecimal((this.PercentChange24h / 100) + 1);
        public decimal YesterdayValueInPreferredFiatCurrency => this.TotalAmount * this.YesterdayPriceInPreferredFiatCurrency;
        public decimal ValueChange24hInPreferredFiatCurrency => this.CurrentValueInPreferredFiatCurrency - this.YesterdayValueInPreferredFiatCurrency;
    }
}