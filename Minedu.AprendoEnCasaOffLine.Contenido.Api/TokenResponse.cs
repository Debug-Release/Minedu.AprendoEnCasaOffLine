using Newtonsoft.Json;

namespace Minedu.IS4.Security.Auth
{
    public class TokenResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? expires_in { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string access_token { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string token_type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string refresh_token { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string error_description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string error { get; set; }
    }
}
