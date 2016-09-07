using Newtonsoft.Json;


namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("reactions")]
    public class ChannelFeedReactions
    {
        [JsonProperty("endorse")]
        public ChannelFeedEndorse Endorse { get; set; }
    }
}
