using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streamhelper.Twitch.API.Helper
{
    public class TwitchScopes
    {
        public static string ConvertToString(List<Enums.Scopes> scopes)
        {
            if (scopes.Count > 0)
                return string.Join("+", scopes.Select(x => x.ToString()));
            else
                return null;
        }
    }
}
