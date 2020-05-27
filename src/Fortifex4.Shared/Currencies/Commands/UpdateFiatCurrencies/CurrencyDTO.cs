using System;
using Fortifex4.Application.Enums;

namespace Fortifex4.Application.Currencies.Commands.UpdateFiatCurrencies
{
    public class CurrencyDTO
    {
        public int CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public DateTimeOffset LastUpdated { get; set; }

        public UpdateStatus UpdateStatus { get; set; }
    }
}