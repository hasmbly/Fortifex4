namespace Fortifex4.Shared.Common
{
    public class SchemeProvider
    {
        public string Value { get; private set; }

        private SchemeProvider(string value) { this.Value = value; }

        public static SchemeProvider Fortifex { get { return new SchemeProvider("Fortifex"); } }
        public static SchemeProvider Google { get { return new SchemeProvider("Google"); } }
        public static SchemeProvider Facebook { get { return new SchemeProvider("Facebook"); } }

        public override string ToString()
        {
            return this.Value;
        }

        public static implicit operator string(SchemeProvider schemeProvider) { return schemeProvider.Value; }
    }
}