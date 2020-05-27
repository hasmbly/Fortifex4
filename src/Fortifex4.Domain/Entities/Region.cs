namespace Fortifex4.Domain.Entities
{
    public class Region
    {
        public int RegionID { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }

        public Country Country { get; set; }
    }

    public static class RegionName
    {
        public const string Undefined = "Undefined";
        public const string Default = Undefined;
    }
}