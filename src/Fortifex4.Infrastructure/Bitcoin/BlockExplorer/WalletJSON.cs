namespace Fortifex4.Infrastructure.Bitcoin.BlockExplorer
{
    public class WalletJSON
    {
        public string addrStr { get; set; }
        public decimal balance { get; set; }
        public decimal balanceSat { get; set; }
        public decimal totalReceived { get; set; }
        public decimal totalReceivedSat { get; set; }
    }
}