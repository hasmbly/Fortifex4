namespace Fortifex4.Application.Common.Interfaces.Ethereum
{
    public class EthereumTransaction
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public decimal Amount { get; set; }
        public string Hash { get; set; }
        public long TimeStamp { get; set; }
    }
}