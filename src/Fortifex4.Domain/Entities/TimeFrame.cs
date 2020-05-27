namespace Fortifex4.Domain.Entities
{
    public class TimeFrame
    {
        public int TimeFrameID { get; set; }
        public string Name { get; set; }
    }

    public static class TimeFrameID
    {
        public const int OneHour = 1;
        public const int OneDay = 2;
        public const int OneWeek = 3;
        public const int LifeTime = 4;

        public const int Default = OneDay;
    }

    public static class TimeFrameName
    {
        public const string OneHour = "1h";
        public const string OneDay = "24h";
        public const string OneWeek = "7d";
        public const string LifeTime = "Lifetime";
    }
}