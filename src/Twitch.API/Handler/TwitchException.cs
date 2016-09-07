using System;

namespace Streamhelper.Twitch.API.Handler
{
    public class TwitchException : Exception
    {
        /// <summary>
        ///  ErrorID is build for multiple situations.
        /// </summary>
        public int ErrorID { get; private set; }

        /// <summary>
        ///  ErrorType is build as an more powerful way to interact with exceptions
        /// </summary>
        public Enums.TwitchExceptionType ErrorType { get; private set; }


        #region Error_ID_And_Type_Connection
        #region ErrorType_Authenticated
        /// <summary>
        ///     ErrorType.Authenticated includes the following Error IDs
        ///     1 -> UserName not Set
        ///     2 -> Parameter not Set
        ///     3 -> Parameter and UserName not Set
        ///     4 -> UserData not found
        ///     50 -> Delay must between 10 and 900
        ///     999 -> Unknown Error
        /// </summary>
        #endregion
        #region ErroType_Authentication
        /// <summary>
        ///     ErrorType.Authentication includes the following Error IDs
        ///     1 -> Unauthorized token (Maybe missed scope or invalid token)
        ///     2 -> Token not valid
        /// </summary>
        #endregion
        #endregion

        public TwitchException()
        {

        }

        public TwitchException(String message, Enums.TwitchExceptionType type ,int id) : this(message, null)
        {
            this.ErrorID = id;
            this.ErrorType = type;
        }

        public TwitchException(String message, Enums.TwitchExceptionType type, int id, Exception inner) : base(message, inner)
        {
            this.ErrorID = id;
            this.ErrorType = type;
        }

        public TwitchException(string message) : this(message, null)
        {

        }

        public TwitchException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
