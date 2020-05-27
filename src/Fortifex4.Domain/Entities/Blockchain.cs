using System.Collections.Generic;
using Fortifex4.Domain.Common;

namespace Fortifex4.Domain.Entities
{
    public class Blockchain : AuditableEntity
    {
        public int BlockchainID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }

        public IList<Currency> Currencies { get; private set; }
        public IList<Wallet> Wallets { get; private set; }
        public IList<Project> Projects { get; private set; }

        public Blockchain()
        {
            this.Currencies = new List<Currency>();
            this.Wallets = new List<Wallet>();
            this.Projects = new List<Project>();
        }
    }

    public static class BlockchainID
    {
        public const int Fiat = 0;
    }

    public static class BlockchainSymbol
    {
        public const string Fiat = "--";
    }

    public static class BlockchainName
    {
        public const string Fiat = "Fiat";
    }

    public static class BlockchainRank
    {
        public const int Fiat = 0;
    }
}