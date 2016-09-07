using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    public class RootResult
    {
        [JsonProperty("token")]
        public Token Token { get; set; }
    }
}