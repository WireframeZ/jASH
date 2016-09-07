using System.Collections.Generic;
using Newtonsoft.Json;

namespace Streamhelper.Twitch.API.Templates.v3
{
    [JsonObject("emoticons")]
    public class Emoticon
    {
        [JsonProperty("regex")]
        public string Regex { get; set; }
        [JsonProperty("images")]
        public List<Image> Images { get; set; }
    }
}