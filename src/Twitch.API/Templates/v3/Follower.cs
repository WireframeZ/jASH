using System;
using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("follows")]
    public class Follower
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("notifications")]
        public bool Notifications { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
    }
}