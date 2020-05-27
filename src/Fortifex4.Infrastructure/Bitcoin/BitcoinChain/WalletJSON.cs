namespace Fortifex4.Infrastructure.Bitcoin.BitcoinChain
{
    public class WalletJSON
    {
        public string address { get; set; }
        public decimal balance { get; set; }
        public string hash_160 { get; set; }
        public decimal total_rec { get; set; }
        public int transactions { get; set; }
        public int unconfirmed_transactions_count { get; set; }
    }
}