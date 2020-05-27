using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Owners.Common
{
    public class WalletDTO
    {
        public int WalletID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string BlockchainName { get; set; }
        public string MainPocketCurrencySymbol { get; set; }
        public string MainPocketCurrencyName { get; set; }
        public CurrencyType MainPocketCurrencyType { get; set; }
        public decimal MainPocketCurrencyUnitPriceInUSD { get; set; }
        public decimal MainPocketBalance { get; set; }

        public WalletContainer Container { get; set; }

        public string MainPocketBalanceDisplayText
        {
            get
            {
                return this.MainPocketBalance.ToString("N4").Replace(".0000", "");
            }
        }

        public decimal MainPocketBalanceInPreferredFiatCurrency
        {
            get
            {
                if (this.Container.MemberPreferredFiatCurrencyUnitPriceInUSD > 0)
                    return this.MainPocketBalance * (this.MainPocketCurrencyUnitPriceInUSD / this.Container.MemberPreferredFiatCurrencyUnitPriceInUSD);
                else
                    return 0m;
            }
        }

        public string MainPocketBalanceInPreferredFiatCurrencyDisplayText
        {
            get
            {
                return this.MainPocketBalanceInPreferredFiatCurrency.ToString("N4").Replace(".0000", "");
            }
        }

        public decimal MainPocketBalanceInPreferredCoinCurrency
        {
            get
            {
                if (this.Container.MemberPreferredCoinCurrencyUnitPriceInUSD > 0)
                    return this.MainPocketBalance * (this.MainPocketCurrencyUnitPriceInUSD / this.Container.MemberPreferredCoinCurrencyUnitPriceInUSD);
                else
                    return 0m;
            }
        }

        public string MainPocketBalanceInPreferredCoinCurrencyDisplayText
        {
            get
            {
                return this.MainPocketBalanceInPreferredCoinCurrency.ToString("N4").Replace(".0000", "");
            }
        }
    }
}