using System.Collections.Generic;

namespace Fortifex4.Domain.Entities
{
    public class Gender
    {
        public int GenderID { get; set; }
        public string Name { get; set; }

        public IList<Member> Members { get; private set; }

        public Gender()
        {
            this.Members = new List<Member>();
        }
    }

    public static class GenderID
    {
        public const int Undefined = 1;
        public const int Male = 2;
        public const int Female = 3;

        public const int Default = Undefined;
    }

    public static class GenderName
    {
        public const string Undefined = "Undefined";
        public const string Male = "Male";
        public const string Female = "Female";
    }
}