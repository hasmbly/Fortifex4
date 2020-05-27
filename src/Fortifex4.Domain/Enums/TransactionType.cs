namespace Fortifex4.Domain.Enums
{
    public enum TransactionType
    {
        StartingBalance = 0,
        BuyIN = 1,
        BuyOUT = -1,
        BuyOUTNonWithholding = -10,
        SellIN = 2,
        SellOUT = -2,
        SellINNonWithholding = 20,
        Deposit = 3,
        Withdrawal = -3,
        ExternalTransferIN = 4,
        ExternalTransferOUT = -4,
        InternalTransferIN = 5,
        InternalTransferOUT = -5,
        SyncTransactionIN = 6,
        SyncTransactionOUT = -6,
        SyncBalanceImport = 60
    }
}