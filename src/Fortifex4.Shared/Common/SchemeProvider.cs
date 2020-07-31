using System;

namespace Fortifex4.Shared.Common
{
    public struct SchemeProvider : IEquatable<SchemeProvider>
    {
        public static SchemeProvider Fortifex { get { return new SchemeProvider("Fortifex"); } }
        public static SchemeProvider Google { get { return new SchemeProvider("Google"); } }
        public static SchemeProvider Facebook { get { return new SchemeProvider("Facebook"); } }

        public string Value { get; private set; }

        private SchemeProvider(string value) { this.Value = value; }

        public override string ToString() { return this.Value; }

        public bool Equals(SchemeProvider anotherSchemeProvider) { return this.Value == anotherSchemeProvider.Value; }

        public override bool Equals(object anotherObject)
        {
            if (anotherObject is SchemeProvider anotherSchemeProvider)
            {
                return this.Value == anotherSchemeProvider.Value;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Value);
        }

        public static bool operator ==(SchemeProvider left, SchemeProvider right) { return left.Equals(right); }
        public static bool operator !=(SchemeProvider left, SchemeProvider right) { return !(left == right); }
        public static implicit operator string(SchemeProvider schemeProvider) { return schemeProvider.Value; }

        public static SchemeProvider FromString(string value)
        {
            return new SchemeProvider(value);
        }
    }
}