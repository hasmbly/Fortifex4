namespace Fortifex4.Infrastructure.Ethereum.Ethplorer
{
    public class TransactionJSON
    {
        public long timestamp { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string hash { get; set; }
        public decimal value { get; set; }
        public bool success { get; set; }
    }
}