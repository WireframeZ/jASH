using System;
using Newtonsoft.Json;
using Streamhelper.Twitch.API.Enums;

namespace Streamhelper.Twitch.API.Templates.v3
{
    public class TwitchDefaultResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("_total")]
        public long Total { get; set; }

        [JsonProperty("_cursor")]
        public long Cursor { get; set; } = 0;

        public TwitchDefaultResponse() {}

        public State GetState()
        {
            State state;
            switch (Status)
            {
                case 204:
                    state = State.successful;
                    break;
                case 404:
                    state = State.notfound;
                    break;
                case 422:
                    state = State.failed;
                    break;
                case 503:
                    state = State.failed;
                    break;
                default:
                    state = State.failed;
                    break;
            }
            return state;
        }
    }
}