using Fortifex4.Domain.Enums;

namespace Fortifex4.Shared.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class CurrencyDTO
    {
        public int CurrencyID { get; set; }
        public string BlockchainName { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public string NameWithSymbol
        {
            get
            {
                return $"{this.Name} ({this.Symbol})";
            }
        }
    }
}