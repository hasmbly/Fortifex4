using Fortifex4.Domain.Enums;

namespace Fortifex4.Shared.Owners.Common
{
    public class WalletDTO : WalletContainer
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

        public string MainPocketBalanceDisplayText
        {
            get
            {
                return this.MainPocketBalance.ToString("N4").Replace(".0000", "");
            }
        }

        public decimal MainPocketBalanceInPreferredFiatCurrency { get; set; }

        public string MainPocketBalanceInPreferredFiatCurrencyDisplayText
        {
            get
            {
                return this.MainPocketBalanceInPreferredFiatCurrency.ToString("N4").Replace(".0000", "");
            }
        }

        public decimal MainPocketBalanceInPreferredCoinCurrency { get; set; }

        public string MainPocketBalanceInPreferredCoinCurrencyDisplayText
        {
            get
            {
                return this.MainPocketBalanceInPreferredCoinCurrency.ToString("N4").Replace(".0000", "");
            }
        }
    }
}