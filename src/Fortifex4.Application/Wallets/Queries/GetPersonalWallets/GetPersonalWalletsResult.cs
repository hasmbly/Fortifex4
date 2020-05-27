using System.Collections.Generic;

namespace Fortifex4.Application.Wallets.Queries.GetPersonalWallets
{
    public class GetPersonalWalletsResult
    {
        public string MemberPreferredFiatCurrencySymbol { get; set; }
        public decimal MemberPreferredFiatCurrencyUnitPriceInUSD { get; set; }
        public string MemberPreferredCoinCurrencySymbol { get; set; }
        public decimal MemberPreferredCoinCurrencyUnitPriceInUSD { get; set; }

        public IList<WalletDTO> PersonalWallets { get; set; }

        public GetPersonalWalletsResult()
        {
            this.PersonalWallets = new List<WalletDTO>();
        }
    }
}