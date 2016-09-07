using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("emotes")]
    public class ChannelFeedCreateEmoteReaction 
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("emote_id")]
        public string Emote_id { get; set; }
        [JsonProperty("created_at")]
        public string Created_at { get; set; }
        [JsonProperty("User")]
        public User User { get; set; }

    }
}
