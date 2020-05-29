using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Owners.Common
{
    public abstract class WalletContainer : GeneralResponse
    {
        public int MemberPreferredFiatCurrencyID { get; set; }
        public string MemberPreferredFiatCurrencySymbol { get; set; }
        public decimal MemberPreferredFiatCurrencyUnitPriceInUSD { get; set; }
        public int MemberPreferredCoinCurrencyID { get; set; }
        public string MemberPreferredCoinCurrencySymbol { get; set; }
        public decimal MemberPreferredCoinCurrencyUnitPriceInUSD { get; set; }
    }
}