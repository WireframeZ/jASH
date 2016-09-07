﻿using System;
using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("subscriptions")]
    public class Subscription : TwitchDefaultResponse
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        // user or channel is null. There are two types of subscription: user subscriptions and channel subscriptions
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("channel")]
        public Channel Channel { get; set; }
    }
}