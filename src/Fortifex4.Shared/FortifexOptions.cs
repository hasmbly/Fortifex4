namespace Fortifex4.Shared
{
    public class FortifexOptions
    {
        public const string RootSection = "Fortifex";

        public string FortifexAdministrator { get; set; }
        public string ProjectDocumentsRootFolderPath { get; set; }
        public int ProjectDocumentsLimit { get; set; }
        public int FileSizeLimit { get; set; }
        public int WalletSynchronizationMillisecondsDelay { get; set; }
        public string EmailServiceProvider { get; set; }
        public string FiatServiceProvider { get; set; }
        public string CryptoServiceProvider { get; set; }
        public string EthereumServiceProvider { get; set; }
        public string BitcoinServiceProvider { get; set; }
        public string DogecoinServiceProvider { get; set; }
        public string SteemServiceProvider { get; set; }
        public string HiveServiceProvider { get; set; }
    }
}