﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("streams")]
    public class Stream
    {
        [JsonProperty("game")]
        public string Game { get; set; }
        [JsonProperty("viewers")]
        public long Viewers { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("_id")]
        public long Id { get; set; }
        [JsonProperty("channel")]
        public Channel Channel { get; set; }
        [JsonProperty("preview")]
        public ScaledImage Preview { get; set; }
        [JsonProperty("average_fps")]
        public double AverageFps { get; set; }
        [JsonProperty("video_height")]
        public double VideoHeight { get; set; }
    }
}
