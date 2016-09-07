using System;
using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("follows")]
    public class FollowedChannel : TwitchDefaultResponse
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("notifications")]
        public bool Notifications { get; set; }
        [JsonProperty("channel")]
        public Channel Channel { get; set; } 
    }
}