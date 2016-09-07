using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streamhelper.Twitch.API.Helper
{
    public class TwitchApplication
    {
        /// <summary>
        ///  Your Application Client id on https://www.twitch.tv/settings/connections
        /// </summary>
        public String clientID { get; set; } = "";
        /// <summary>
        ///  Your Application Client Secret id on https://www.twitch.tv/settings/connections
        /// </summary>
        public String clientSecret { get; set; } = "";
        /// <summary>
        ///  Your Application Redirect URI on https://www.twitch.tv/settings/connections
        /// </summary>
        public String redirectedUri { get; set; } = "";

        public TwitchApplication(String clientid, String clientsecret, String redirectedUri)
        {
            this.clientID = clientid;
            this.clientSecret = clientsecret;
            this.redirectedUri = redirectedUri;
        }
    }
}
