using System.Collections.Generic;

namespace Fortifex4.Application.Common.Interfaces
{
    public class CryptoWallet
    {
        public decimal Balance { get; set; }
        public IList<CryptoPocket> Pockets { get; set; }

        public CryptoWallet()
        {
            this.Pockets = new List<CryptoPocket>();
        }
    }
}