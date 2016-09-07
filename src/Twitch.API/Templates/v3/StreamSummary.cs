using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    public class StreamSummary : TwitchDefaultResponse
    {
        [JsonProperty("viewers")]
        public long Viewers { get; set; }

        [JsonProperty("channels")]
        public long Channels { get; set; }
    }
}