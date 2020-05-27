﻿namespace Fortifex4.Application
{
    public class FortifexOptions
    {
        public const string RootSection = "Fortifex";

        public int WalletSynchronizationMillisecondsDelay { get; set; }
        public string EmailServiceProvider { get; set; }
        public string FiatServiceProvider { get; set; }
        public string CryptoServiceProvider { get; set; }
        public string EthereumServiceProvider { get; set; }
        public string BitcoinServiceProvider { get; set; }
        public string DogecoinServiceProvider { get; set; }
    }
}