using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("endorse")]
    public class ChannelFeedEndorse
    {
        [JsonProperty("emote")]
        public string Emote { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("user_ids")]
        public int[] User_ids { get; set; }
    }
}
