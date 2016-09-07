using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("emotes")]
    public class ChannelFeedEmotes
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("start")]
        public string Start { get; set; }
        [JsonProperty("end")]
        public string End { get; set; }
    }
}
