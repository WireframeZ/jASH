using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("notifications")]
    public class Notification
    {
        [JsonProperty("email")]
        public bool Email { get; set; }
        [JsonProperty("push")]
        public bool Push { get; set; }
    }
}