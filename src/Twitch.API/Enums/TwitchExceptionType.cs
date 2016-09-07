using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streamhelper.Twitch.API.Enums
{
    public enum TwitchExceptionType
    {
        /// <summary>
        ///  Authenticated is called if something is wrong with the internal Authenticated class
        /// </summary>
        Authenticated,
        /// <summary>
        ///  Authentication is called if something is wrong with the Authentication 
        /// </summary>
        Authentication,
        /// <summary>
        ///  Connection is called if the twitch connection is broken
        /// </summary>
        Connection
    }
}
