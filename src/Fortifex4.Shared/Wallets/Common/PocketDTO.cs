namespace Fortifex4.Shared.Wallets.Common
{
    public class PocketDTO
    {
        public int PocketID { get; set; }
        public int CurrencyID { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
    }
}