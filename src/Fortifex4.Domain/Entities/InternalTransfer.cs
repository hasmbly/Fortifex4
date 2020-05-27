namespace Fortifex4.Domain.Entities
{
    public class InternalTransfer
    {
        public int InternalTransferID { get; set; }
        public int FromTransactionID { get; set; }
        public int ToTransactionID { get; set; }

        public Transaction FromTransaction { get; set; }
        public Transaction ToTransaction { get; set; }
    }
}