using Newtonsoft.Json;

namespace Fortifex4.Infrastructure.Steem.Steemit
{
    public class GetAccountsRequestJSON
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }

        [JsonProperty("params")]
        public string[][] Params { get; set; }

        public int id { get; set; }

        public GetAccountsRequestJSON(string username)
        {
            this.jsonrpc = "2.0";
            this.method = "condenser_api.get_accounts";
            string[] usernames = new string[] { username };
            this.Params = new string[][] { usernames };
            this.id = 1;
        }
    }
}