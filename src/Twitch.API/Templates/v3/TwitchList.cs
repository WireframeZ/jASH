using System.Collections.Generic;
using Newtonsoft.Json;
using Streamhelper.Twitch.API.Helpers;

namespace Streamhelper.Twitch.API.Templates.v3
{

    [JsonObject(ItemConverterType = typeof (TwitchListConverter))]
    public class TwitchList<T> : TwitchDefaultResponse
    {
        public List<T> List { get; set; }
    }
}