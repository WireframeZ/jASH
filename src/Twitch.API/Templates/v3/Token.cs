using System.Net;
using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("token")]
    public class Token
    {
        [JsonProperty("authorization")]
        public Authorization Authorization { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("valid")]
        public bool Valid { get; set; }
    }
}