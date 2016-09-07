using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    public class StreamAPIResult : TwitchDefaultResponse
    {
        [JsonProperty("stream")]
        public Stream Stream { get; set; }

    }
}