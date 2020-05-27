using System.Collections.Generic;

namespace Fortifex4.Domain.Entities
{
    public class Country
    {
        public string CountryCode { get; set; }
        public string Name { get; set; }

        public IList<Region> Regions { get; private set; }

        public Country()
        {
            this.Regions = new List<Region>();
        }
    }

    public static class CountryCode
    {
        public const string Undefined = "Undefined";
    }
}