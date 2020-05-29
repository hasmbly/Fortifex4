using Fortifex4.Domain.Enums;

namespace Fortifex4.Shared.Wallets.Queries.GetPersonalWallets
{
    public class WalletDTO
    {
        public int WalletID { get; set; }
        public int BlockchainID { get; set; }
        public int OwnerID { get; set; }
        public ProviderType ProviderType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsSynchronized { get; set; }

        public string BlockchainSymbol { get; set; }
        public string BlockchainName { get; set; }
        public decimal MainPocketCurrencyUnitPriceInUSD { get; set; }
        public decimal MainPocketBalance { get; set; }

        public GetPersonalWalletsResponse Container { get; set; }

        public string IsSynchronizedDisplayText
        {
            get
            {
                return this.IsSynchronized ? "Yes" : "No";
            }
        }

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