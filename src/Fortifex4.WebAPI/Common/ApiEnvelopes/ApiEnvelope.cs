using Newtonsoft.Json;

namespace Fortifex4.WebAPI.Common.ApiEnvelopes
{
    public abstract class ApiEnvelope
    {
        public Status Status { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Result { get; set; }
    }

    public class Status
    {
        public bool IsError { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}