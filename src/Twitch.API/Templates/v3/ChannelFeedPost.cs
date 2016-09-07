using Newtonsoft.Json;
using System.Collections.Generic;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("posts")]
    public class ChannelFeedPost : TwitchDefaultChannelFeedResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("created_at")]
        public string Created_at { get; set; }
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
        [JsonProperty("emotes")]
        public List<ChannelFeedEmotes> Emotes { get; set; }
        [JsonProperty("reactions")]
        public ChannelFeedReactions Reactions { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
    }
}
