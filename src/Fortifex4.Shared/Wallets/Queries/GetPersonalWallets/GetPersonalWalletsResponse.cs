using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Wallets.Queries.GetPersonalWallets
{
    public class GetPersonalWalletsResponse : GeneralResponse
    {
        public string MemberPreferredFiatCurrencySymbol { get; set; }
        public decimal MemberPreferredFiatCurrencyUnitPriceInUSD { get; set; }
        public string MemberPreferredCoinCurrencySymbol { get; set; }
        public decimal MemberPreferredCoinCurrencyUnitPriceInUSD { get; set; }

        public IList<WalletDTO> PersonalWallets { get; set; }

        public GetPersonalWalletsResponse()
        {
            this.PersonalWallets = new List<WalletDTO>();
        }
    }
}