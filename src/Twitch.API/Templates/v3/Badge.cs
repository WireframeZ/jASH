using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("badges")]
    public class Badge
    {
        [JsonProperty("alpha")]
        public string Alpha { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("svg")]
        public string Svg { get; set; }
    }
}