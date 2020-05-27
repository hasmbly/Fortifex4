namespace Fortifex4.Infrastructure.Bitcoin.SoChain
{
    public class WalletJSON
    {
        public string status { get; set; }
        public DataJSON data { get; set; }
    }

    public class DataJSON
    {
        public decimal confirmed_balance { get; set; }
    }
}