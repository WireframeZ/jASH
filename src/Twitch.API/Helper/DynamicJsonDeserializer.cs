using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using Streamhelper.Twitch.API.Helpers;

namespace Streamhelper.Twitch.API.Helper
{
    // @author gibletto - https://github.com/gibletto
    public class DynamicJsonDeserializer : IDeserializer
    {
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Converters = new JsonConverter[] { new TwitchListConverter() } });
        }
    }
}
