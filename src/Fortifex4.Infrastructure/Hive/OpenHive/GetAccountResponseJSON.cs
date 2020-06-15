using System.Collections.Generic;

namespace Fortifex4.Infrastructure.Hive.OpenHive
{
    public class GetAccountsResponseJSON
    {
        public IList<AccountJSON> result { get; set; }

        public GetAccountsResponseJSON()
        {
            this.result = new List<AccountJSON>();
        }
    }

    public class AccountJSON
    {
        public string balance { get; set; }
    }
}