using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("authorization")]
    public class Authorization
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; } 
    }
}