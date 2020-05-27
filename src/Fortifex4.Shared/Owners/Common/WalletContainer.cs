namespace Fortifex4.Application.Owners.Common
{
    public abstract class WalletContainer
    {
        public int MemberPreferredFiatCurrencyID { get; set; }
        public string MemberPreferredFiatCurrencySymbol { get; set; }
        public decimal MemberPreferredFiatCurrencyUnitPriceInUSD { get; set; }
        public int MemberPreferredCoinCurrencyID { get; set; }
        public string MemberPreferredCoinCurrencySymbol { get; set; }
        public decimal MemberPreferredCoinCurrencyUnitPriceInUSD { get; set; }
    }
}