using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("deleted")]
    public class ChannelFeedDeleted : TwitchDefaultResponse
    {
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}
