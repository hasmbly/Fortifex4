using Fortifex4.Domain.Enums;
using System.Collections.Generic;

namespace Fortifex4.Domain.Entities
{
    public class Provider
    {
        public int ProviderID { get; set; }
        public string Name { get; set; }
        public ProviderType ProviderType { get; set; }
        public string SiteURL { get; set; }

        public IList<Owner> Owners { get; private set; }

        public Provider()
        {
            this.Owners = new List<Owner>();
        }
    }

    public static class ProviderID
    {
        public const int Personal = 0;
    }

    public static class ProviderName
    {
        public const string Personal = "Personal Wallet";
    }
}