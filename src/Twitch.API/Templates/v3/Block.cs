﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("blocks")]
    public class Block
    {
        [JsonProperty("_id")]
        public long Id { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
    }
}