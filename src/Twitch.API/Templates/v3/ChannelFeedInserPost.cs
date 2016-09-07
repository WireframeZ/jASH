using Newtonsoft.Json;
using System.Collections.Generic;


namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("posts")]
    public class ChannelFeedInserPost : TwitchDefaultChannelFeedResponse
    {
            [JsonProperty("tweet")]
            public string Tweet { get; set; }
            [JsonProperty("post")]
            public ChannelFeedPost Post { get; set; }
    }
}
