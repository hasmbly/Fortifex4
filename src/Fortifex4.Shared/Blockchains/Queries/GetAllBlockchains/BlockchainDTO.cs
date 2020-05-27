namespace Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains
{
    public class BlockchainDTO 
    {
        public int BlockchainID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }

        public string NameWithSymbol
        {
            get
            {
                return $"{this.Name} ({this.Symbol})";
            }
        }
    }
}