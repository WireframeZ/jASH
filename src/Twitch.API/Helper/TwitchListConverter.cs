﻿using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Streamhelper.Twitch.API.Helpers
{
    // @author gibletto
    class TwitchListConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = Activator.CreateInstance(objectType) as Templates.v3.TwitchDefaultResponse;
            var genericArg = objectType.GetGenericArguments()[0];
            var key = genericArg.GetCustomAttribute<JsonObjectAttribute>();
            if (value == null || key == null)
                return null;
            var jsonObject = JObject.Load(reader);
            value.Total = SetValue<long>(jsonObject["_total"]);
            value.Error = SetValue<string>(jsonObject["error"]);
            value.Message = SetValue<string>(jsonObject["message"]);
            try
            {
                value.Cursor = SetValue<long>(jsonObject["_cursor"]);
            }
            catch { }

            var list = jsonObject[key.Id];
            var prop = value.GetType().GetProperty("List");
            if (prop != null && list != null)
            {
                prop.SetValue(value, list.ToObject(prop.PropertyType, serializer));
            }
            return value;
        }


        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && typeof(Templates.v3.TwitchList<>) == objectType.GetGenericTypeDefinition();
        }

        private T SetValue<T>(JToken token)
        {
            if (token != null)
            {
                return (T)token.ToObject(typeof(T));
            }
            return default(T);
        }
    }
}