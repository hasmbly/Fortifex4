using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain
{
    public class WalletDTO
    {
        public int WalletID { get; set; }
        public int PocketID { get; set; }
        public string Name { get; set; }
        public ProviderType ProviderType { get; set; }
        public string OwnerProviderName { get; set; }

        public string ProviderNameWithWalletName => $"{this.OwnerProviderName} - {this.Name}";
    }
}