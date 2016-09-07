using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streamhelper.Twitch.API.Enums
{
    public enum Scopes
    {
        user_read,
        user_blocks_edit,
        user_blocks_read,
        user_follows_edit,
        channel_read,
        channel_editor,
        channel_commercial,
        channel_stream,
        channel_subscriptions,
        user_subscriptions,
        channel_check_subscription,
        chat_login,
        channel_feed_read,
        channel_feed_edit
    }
}
