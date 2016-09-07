using RestSharp;
using System;
using System.Threading.Tasks;
using Streamhelper.Twitch.API.Helper;


namespace Streamhelper.Twitch.API.Handler.Connecter
{
    public class Authenticated : Anonymous
    {
        private readonly String oauthToken;
        public String userName { get; private set; } = null;


        /// <summary>
        ///  Required Scope (user_read)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="appData"></param>
        public Authenticated(String token, TwitchApplication appData) : base(appData)
        {
            this.oauthToken = token;
            CheckToken(true);
        }

        private void CheckToken(bool setUsername)
        {
            var req = GetAuthenticatedSubmitRequest("", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);
            var resp = Client.Execute<Templates.v3.RootResult>(req);

            if (resp.Data.Token.Valid)
            {
                if (setUsername)
                    this.userName = resp.Data.Token.UserName;
            }
            else
            {
                throw new TwitchException("Token not valid", Enums.TwitchExceptionType.Authentication, 2);
            }
        }

        #region Async

        #region ChannelNode


        /// <summary>
        ///     Get your Channel Information
        /// </summary>
        /// <returns></returns>
        public Task<Templates.v3.Channel> GetMyChannelAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.Channel>();
            var req = GetAuthenticatedSubmitRequest("channel", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);


            Client.ExecuteAsync<Templates.v3.Channel>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;
        }

        /// <summary>
        ///     Get your channel Editors
        /// </summary>
        /// <returns></returns>
        public Task<Templates.v3.TwitchList<Templates.v3.User>> GetChannelEditorsAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.User>>();
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/editors", Method.GET);
            req.AddUrlSegment("channel", userName);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.User>>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Update Channel Informations
        /// </summary>
        /// <param name="status"></param>
        /// <param name="game"></param>
        /// <param name="delay"></param>
        /// <param name="lang"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        public Task<Templates.v3.Channel> UpdateChannelAsync(string status = null, string game = null, int delay = -1, Enums.BroadcasterLanguages lang = Enums.BroadcasterLanguages.NotSet, Enums.ChannelFeed feed = Enums.ChannelFeed.NotSet)
        {
            var tcs = new TaskCompletionSource<Templates.v3.Channel>();
            // Main JsonObject Container
            JsonObject jobj = new JsonObject();

            // Channel Node JsonObject Container
            JsonObject channel = new JsonObject();

            var req = GetAuthenticatedSubmitRequest("channels/{channel}", Method.PUT);
            req.AddUrlSegment("channel", userName);
            req.RequestFormat = DataFormat.Json;

            if (!string.IsNullOrEmpty(status))
                channel.Add("status", status.Replace(' ', '+'));

            if (!string.IsNullOrEmpty(game))
                channel.Add("game", game);

            if (delay >= 10 || delay <= 900)
            {
                channel.Add("delay", delay.ToString());
            }
            else
            {
                if (delay != -1)
                    throw new TwitchException("Delay must between 10 and 900", Enums.TwitchExceptionType.Authenticated, 50);
            }

            if (feed != Enums.ChannelFeed.NotSet)
                channel.Add("channel_feed_enabled", feed.ToString().ToLower());

            if (lang != Enums.BroadcasterLanguages.NotSet)
                channel.Add("broadcaster_language", lang.ToString().ToLower());


            jobj.Add("channel", channel);
            req.AddBody(jobj);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.Channel>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Set Channel Title / Status
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<Templates.v3.Channel> SetTitleAsync(string title)
        {
            return await UpdateChannelAsync(title);
        }

        /// <summary>
        ///     Set Channel Game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task<Templates.v3.Channel> SetGameAsync(string game)
        {
            return await UpdateChannelAsync(null, game);
        }

        /// <summary>
        ///  Partner Channels Only!
        ///  Set your Channel Delay in minutes
        /// </summary>
        /// <param name="delay">Your Delay in Minutes</param>
        /// <returns></returns>
        public async Task<Templates.v3.Channel> SetDelayAsync(int delay)
        {
            return await UpdateChannelAsync(null, null, delay);
        }

        /// <summary>
        ///     Set your Channel Language
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public async Task<Templates.v3.Channel> SetLangAsync(Enums.BroadcasterLanguages lang)
        {
            return await UpdateChannelAsync(null, null, -1, lang);
        }

        /// <summary>
        ///     Disavle or Enable your Channel Feed feature
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public async Task<Templates.v3.Channel> SetChangeChannelFeedAsync(Enums.ChannelFeed feed)
        {
            return await UpdateChannelAsync(null, null, -1, Enums.BroadcasterLanguages.NotSet, feed);
        }

        /// <summary>
        ///     Reset your Stream Key. IMPORTANT: Remember if you reset your Stream Key your are not able to continue Streaming with your old key
        /// </summary>
        /// <returns></returns>
        public Task<Templates.v3.User> ResetStreamingKeyAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.User>();
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/stream_key", Method.DELETE);
            req.AddUrlSegment("channel", userName);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.User>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }


        /// <summary>
        ///     Partner Channels Only!
        ///     Play Commercial 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public Task<Templates.v3.TwitchDefaultResponse> PlayCommercialAsync(Enums.CommercialLength length)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchDefaultResponse>();
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/stream_key", Method.POST);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("length", length.ToString().Replace("_", ""));
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.TwitchDefaultResponse>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);

            }
        }

        /// <summary>
        ///     Get your Channel Subscribers
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        //TODO Sorting and Paging
        public Task<Templates.v3.TwitchList<Templates.v3.Subscription>> GetSubscribersAsync(Enums.APISorting sort = Enums.APISorting.asc)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Subscription>>();
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/subscriptions", Method.GET);
            req.AddUrlSegment("channel", userName);
            req.AddParameter("direction", sort);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Subscription>>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///  Get a Single Channel Subscriber of your Channel
        /// </summary>
        /// <param name="userToCheck"></param>
        /// <returns></returns>
        public Task<Templates.v3.Subscription> GetSingleSubscriberAsync(string userToCheck)
        {
            var tcs = new TaskCompletionSource<Templates.v3.Subscription>();
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/subscriptions/{user}", Method.GET);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("user", userToCheck);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.Subscription>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToCheck))
                    throw new TwitchException("User to check and UserName not set", Enums.TwitchExceptionType.Authenticated, 3);
                else if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userToCheck))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToCheck))
                    throw new TwitchException("User to check not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        /// <summary>
        ///     Is a your subscribed to your channel
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> IsUserSubscriberAsync(string user)
        {
            Templates.v3.Subscription state = await GetSingleSubscriberAsync(user);
            return state.Status != 404 && state.Status != 422;

        }


        /// <summary>
        ///     Get a single channel you have subscribed
        /// </summary>
        /// <param name="channelToCheck"></param>
        /// <returns></returns>
        public Task<Templates.v3.Subscription> GetSingleSubscribedChannelAsync(string channelToCheck)
        {
            var tcs = new TaskCompletionSource<Templates.v3.Subscription>();
            var req = GetAuthenticatedSubmitRequest("users/{user}/subscriptions/{channel}", Method.GET);
            req.AddUrlSegment("user", userName);
            req.AddUrlSegment("channel", channelToCheck);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.Subscription>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(channelToCheck))
                    throw new TwitchException("Channel to check and UserName not set", Enums.TwitchExceptionType.Authenticated, 3);
                else if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(channelToCheck))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(channelToCheck))
                    throw new TwitchException("Channel to check not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        /// <summary>
        ///     Are you subsribed to a channel?
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task<bool> IsSubscribedToChannelAsync(string channel)
        {
            Templates.v3.Subscription state = await GetSingleSubscribedChannelAsync(channel);
            return state.Status != 404;
        }

        /// <summary>
        ///     Is selected channel Twitch Subscription Partner?
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task<bool> IsSubscriptionPartnerAsync(string channel)
        {
            Templates.v3.Subscription state = await GetSingleSubscribedChannelAsync(channel);
            return state.Status != 422;
        }

        #endregion

        #region UserNode

        /// <summary>
        ///  Get your User Informations
        /// </summary>
        /// <returns></returns>
        public Task<Templates.v3.User> GetMyUserDataAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.User>();
            var req = GetAuthenticatedSubmitRequest("user", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);
            Client.ExecuteAsync<Templates.v3.User>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;
        }

        /// <summary>
        ///     Get Blocked Users
        /// </summary>
        /// <returns></returns>
        public Task<Templates.v3.TwitchList<Templates.v3.Block>> GetMyBlocksAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Block>>();
            var req = GetAuthenticatedSubmitRequest("users/{user}/blocks", Method.GET);
            req.AddUrlSegment("user", userName);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Block>>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///  Block user in your Channel
        /// </summary>
        /// <param name="userToBlock"></param>
        /// <returns></returns>
        public Task<Templates.v3.TwitchDefaultResponse> BlockUserAsync(string userToBlock)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchDefaultResponse>();
            var req = GetAuthenticatedSubmitRequest("users/{user}/blocks/{target}", Method.PUT);
            req.AddUrlSegment("user", userName);
            req.AddUrlSegment("target", userToBlock);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToBlock))
            {
                Client.ExecuteAsync<Templates.v3.TwitchDefaultResponse>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToBlock))
                    throw new TwitchException("UserToBlock and UserName not set", Enums.TwitchExceptionType.Authenticated, 3);
                else if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userToBlock))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToBlock))
                    throw new TwitchException("UserToBlock not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        /// <summary>
        ///  Unblock User from your Channel
        /// </summary>
        /// <param name="userToUnBlock"></param>
        /// <returns></returns>
        public Task<Templates.v3.TwitchDefaultResponse> UnBlockUserAsync(string userToUnBlock)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchDefaultResponse>();
            var req = GetAuthenticatedSubmitRequest("users/{user}/blocks/{target}", Method.DELETE);
            req.AddUrlSegment("user", userName);
            req.AddUrlSegment("target", userToUnBlock);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToUnBlock))
            {
                Client.ExecuteAsync<Templates.v3.TwitchDefaultResponse>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToUnBlock))
                    throw new TwitchException("UserToUnBlock and UserName not set", Enums.TwitchExceptionType.Authenticated, 3);
                else if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userToUnBlock))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToUnBlock))
                    throw new TwitchException("UserToUnBlock not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        /// <summary>
        ///  Follow to Selected Channel
        /// </summary>
        /// <param name="targetChannel"></param>
        /// <param name="notifications"></param>
        /// <returns></returns>
        public Task<Templates.v3.FollowedChannel> FollowAsync(string targetChannel, bool notifications = false)
        {
            var tcs = new TaskCompletionSource<Templates.v3.FollowedChannel>();
            var req = GetAuthenticatedSubmitRequest("users/{user}/follows/channels/{target}", Method.PUT);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("target", targetChannel);
            req.AddParameter("notifications", notifications);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.FollowedChannel>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }


        /// <summary>
        ///  Unfollow to Selected Channel
        /// </summary>
        /// <param name="targetChannel"></param>
        /// <returns></returns>
        public Task<Templates.v3.TwitchDefaultResponse> UnFollowAsync(string targetChannel)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchDefaultResponse>();
            var req = GetAuthenticatedSubmitRequest("users/{user}/follows/channels/{target}", Method.DELETE);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("target", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(targetChannel))
            {
                Client.ExecuteAsync<Templates.v3.TwitchDefaultResponse>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                if (string.IsNullOrEmpty(userName))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if (string.IsNullOrEmpty(targetChannel))
                    throw new TwitchException("Target Channel not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown Error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        #endregion

        #region StreamNode

        /// <summary>
        ///  Get your Stream Informations
        /// </summary>
        /// <returns></returns>
        public Task<Templates.v3.StreamAPIResult> GetMyStreamAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.StreamAPIResult>();
            var req = GetAuthenticatedSubmitRequest("streams/{channel}", Method.GET);
            req.AddUrlSegment("channel", userName);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                Client.ExecuteAsync<Templates.v3.StreamAPIResult>(req, (response) =>
                {
                    tcs.SetResult(response.Data);
                });

                return tcs.Task;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Get your Followed streams
        /// </summary>
        /// <returns></returns>
        //TODO: Sorting and Paging
        public Task<Templates.v3.TwitchList<Templates.v3.Stream>> GetFollowedStreamsAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Stream>>();
            var req = GetAuthenticatedSubmitRequest("streams/followed", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Stream>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;
        }
        #endregion

        #region VideoNode
        /// <summary>
        ///     Get Video Informations of your followed streams
        /// </summary>
        /// <returns></returns>
        //TODO Paging and Sorting
        public Task<Templates.v3.TwitchList<Templates.v3.Video>> GetFollowedVideosAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Video>>();
            var req = GetSubmitRequest("videos/followed", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Video>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;
        }
        #endregion

        #endregion


        /// <summary>
        ///  All Non Async Methods
        /// </summary>
        /// <returns></returns>
        #region NonAsync

        #region Channel_Feed

        /// <summary>
        ///     Get channel feed posts
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Templates.v3.TwitchList<Templates.v3.ChannelFeedPost> GetChannelFeedPosts(int limit)
        {
            var req = GetAuthenticatedSubmitRequest("feed/{channel}/posts", Method.GET);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("limit", limit.ToString());
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.ChannelFeedPost>>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Create post
        /// </summary>
        /// <param name="content"></param>
        /// <param name="share"></param>
        /// <returns></returns>
        public Templates.v3.ChannelFeedInserPost SetChannelFeedPost(string content, Boolean share = false)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var req = GetAuthenticatedSubmitRequest("feed/{channel}/posts", Method.POST);
                req.AddUrlSegment("channel", userName);
                RestAPIVersion(Enums.APIVersion.v3);

                

                if (!string.IsNullOrEmpty(content))
                {
                    req.AddParameter("content", content);
                    req.AddParameter("share", share);

                    var resp = Client.Execute<Templates.v3.ChannelFeedInserPost>(req);
                    return resp.Data;
                }
                else
                {
                    throw new TwitchException("Parameter 'content' not set", Enums.TwitchExceptionType.Authenticated, 2);
                }
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Get post
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public Templates.v3.ChannelFeedPost GetChannelFeedPost(long postid)
        {
            var req = GetAuthenticatedSubmitRequest("feed/{channel}/posts/{id}", Method.GET);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("id", postid.ToString());
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.ChannelFeedPost>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Create reaction to post.
        ///     BUG: UserID is not set on insert of any reaction
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="emote"></param>
        /// <returns></returns>
        // TODO: UserID is not set on insert of any reaction
        public Templates.v3.ChannelFeedCreateEmoteReaction SetChannelFeedReaction(long postId = -1, string emote = "endorse")
        {
            if (!string.IsNullOrEmpty(userName) && postId > -1 )
            {
                var req = GetAuthenticatedSubmitRequest("feed/{channel}/posts/{id}/reactions", Method.POST);
                req.AddUrlSegment("channel", userName);
                req.AddUrlSegment("id", postId.ToString());
                RestAPIVersion(Enums.APIVersion.v3);



                if (!string.IsNullOrEmpty(emote))
                {
                    req.AddParameter("emote", emote);

                    var resp = Client.Execute<Templates.v3.ChannelFeedCreateEmoteReaction>(req);
                    return resp.Data;
                }
                else
                {
                    throw new TwitchException("Parameter 'content' not set", Enums.TwitchExceptionType.Authenticated, 2);
                }
            }
            else
            {
                throw new TwitchException("UserName or PostId not set", Enums.TwitchExceptionType.Authenticated, 3);
            }
        }

        /// <summary>
        ///  Delete reaction
        ///  BUG: UserID is not set on insert of any reaction. You can not Delete reactions by userId ATM
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="emote"></param>
        /// <returns></returns>
        // TODO: UserID is not set on insert of any reaction
        public Templates.v3.ChannelFeedDeleted DeleteChannelFeedReaction(long postId = -1, string emote = "endorse")
        {
            if (!string.IsNullOrEmpty(userName) && postId > -1)
            {
                var req = GetAuthenticatedSubmitRequest("feed/{channel}/posts/{id}/reactions", Method.DELETE);
                req.AddUrlSegment("channel", userName);
                req.AddUrlSegment("id", postId.ToString());
                
                RestAPIVersion(Enums.APIVersion.v3);

                if (!string.IsNullOrEmpty(emote))
                {
                    req.AddParameter("emote", emote);

                    var resp = Client.Execute<Templates.v3.ChannelFeedDeleted>(req);
                    return resp.Data;
                }
                else
                {
                    throw new TwitchException("Parameter 'content' not set", Enums.TwitchExceptionType.Authenticated, 2);
                }
            }
            else
            {
                throw new TwitchException("UserName or PostId not set", Enums.TwitchExceptionType.Authenticated, 3);
            }
        }

        /// <summary>
        ///     Delete post
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public Templates.v3.ChannelFeedPost DeleteChannelFeedPost(long postid)
        {
            var req = GetAuthenticatedSubmitRequest("feed/{channel}/posts/{id}", Method.DELETE);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("id", postid.ToString());
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.ChannelFeedPost>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }


        #endregion

        #region ChannelNode


        /// <summary>
        ///     Get your Channel Information
        /// </summary>
        /// <returns></returns>
        public Templates.v3.Channel GetMyChannel()
        {
            var req = GetAuthenticatedSubmitRequest("channel", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);
            var resp = Client.Execute<Templates.v3.Channel>(req);

            return resp.Data;
        }

        /// <summary>
        ///     Get your channel Editors
        /// </summary>
        /// <returns></returns>
        public Templates.v3.TwitchList<Templates.v3.User> GetChannelEditors()
        {
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/editors", Method.GET);
            req.AddUrlSegment("channel", userName);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.User>>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Update Channel Informations
        /// </summary>
        /// <param name="status"></param>
        /// <param name="game"></param>
        /// <param name="delay"> -1 = Not Set and skip delay. Must between 10 and 900 (Seconds). Channel need to be partner</param>
        /// <param name="lang"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        public Templates.v3.Channel UpdateChannel(string status = null, string game = null, int delay = -1, Enums.BroadcasterLanguages lang = Enums.BroadcasterLanguages.NotSet, Enums.ChannelFeed feed = Enums.ChannelFeed.NotSet)
        {
            // Main JsonObject Container
            JsonObject jobj = new JsonObject();

            // Channel Node JsonObject Container
            JsonObject channel = new JsonObject();

            var req = GetAuthenticatedSubmitRequest("channels/{channel}", Method.PUT);
            req.AddUrlSegment("channel", userName);
            req.RequestFormat = DataFormat.Json;

            if (!string.IsNullOrEmpty(status))
                channel.Add("status", status);

            if (!string.IsNullOrEmpty(game))
                channel.Add("game", game);

            if (delay >= 10 || delay <= 900)
            {
                channel.Add("delay", delay.ToString());
            }
            else
            {
                if (delay != -1)
                    throw new TwitchException("Delay must between 10 and 900", Enums.TwitchExceptionType.Authenticated, 50);
            }

            if (feed != Enums.ChannelFeed.NotSet)
                channel.Add("channel_feed_enabled", feed.ToString().ToLower());

            if (lang != Enums.BroadcasterLanguages.NotSet)
                channel.Add("broadcaster_language", lang.ToString().ToLower());


            jobj.Add("channel", channel);
            req.AddBody(jobj);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.Channel>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Set Channel Title / Status
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Templates.v3.Channel SetTitle(string title)
        {
            return UpdateChannel(title);
        }

        /// <summary>
        ///     Set Channel Game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public Templates.v3.Channel SetGame(string game)
        {
            return UpdateChannel(null, game);
        }

        /// <summary>
        ///  Partner Channels Only!
        ///  Set your Channel Delay in minutes
        /// </summary>
        /// <param name="delay">Your Delay in Minutes</param>
        /// <returns></returns>
        public Templates.v3.Channel SetDelay(int delay)
        {
            return UpdateChannel(null, null, delay);
        }

        /// <summary>
        ///     Set your Channel Language
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public Templates.v3.Channel SetLang(Enums.BroadcasterLanguages lang)
        {
            return UpdateChannel(null, null, -1, lang);
        }

        /// <summary>
        ///     Disavle or Enable your Channel Feed feature
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public Templates.v3.Channel SetChangeChannelFeed(Enums.ChannelFeed feed)
        {
            return UpdateChannel(null, null, -1, Enums.BroadcasterLanguages.NotSet, feed);
        }

        /// <summary>
        ///     Reset your Stream Key. IMPORTANT: Remember if you reset your Stream Key your are not able to continue Streaming with your old key
        /// </summary>
        /// <returns></returns>
        public Templates.v3.User ResetStreamingKey()
        {
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/stream_key", Method.DELETE);
            req.AddUrlSegment("channel", userName);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.User>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }


        /// <summary>
        ///     Partner Channels Only!
        ///     Play Commercial 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public Templates.v3.TwitchDefaultResponse PlayCommercial(Enums.CommercialLength length)
        {
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/stream_key", Method.POST);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("length", length.ToString().Replace("_",""));
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.TwitchDefaultResponse>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Get your Channel Subscribers
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        //TODO Sorting and Paging
        public Templates.v3.TwitchList<Templates.v3.Subscription> GetSubscribers(Enums.APISorting sort = Enums.APISorting.asc)
        {
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/subscriptions", Method.GET);
            req.AddUrlSegment("channel", userName);
            req.AddParameter("direction", sort);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Subscription>>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///  Get a Single Channel Subscriber of your Channel
        /// </summary>
        /// <param name="userToCheck"></param>
        /// <returns></returns>
        public Templates.v3.Subscription GetSingleSubscriber(string userToCheck)
        {
            var req = GetAuthenticatedSubmitRequest("channels/{channel}/subscriptions/{user}", Method.GET);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("user", userToCheck);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.Subscription>(req);
                return resp.Data;
            }
            else
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToCheck))
                    throw new TwitchException("User to check and UserName not set", Enums.TwitchExceptionType.Authenticated, 3);
                else if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userToCheck))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToCheck))
                    throw new TwitchException("User to check not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        /// <summary>
        ///     Is a your subscribed to your channel
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsUserSubscriber(string user)
        {
            int state = GetSingleSubscriber(user).Status;
            return state != 404 && state != 422;
        }

        /// <summary>
        ///     Get a single channel you have subscribed
        /// </summary>
        /// <param name="channelToCheck"></param>
        /// <returns></returns>
        public Templates.v3.Subscription GetSingleSubscribedChannel(string channelToCheck)
        {
            var req = GetAuthenticatedSubmitRequest("users/{user}/subscriptions/{channel}", Method.GET);
            req.AddUrlSegment("user", userName);
            req.AddUrlSegment("channel", channelToCheck);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.Subscription>(req);
                return resp.Data;
            }
            else
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(channelToCheck))
                    throw new TwitchException("Channel to check and UserName not set", Enums.TwitchExceptionType.Authenticated, 3);
                else if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(channelToCheck))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(channelToCheck))
                    throw new TwitchException("Channel to check not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        /// <summary>
        ///     Are you subsribed to a channel?
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool IsSubscribedToChannel(string channel)
        {
            return GetSingleSubscribedChannel(channel).Status != 404;
        }

        /// <summary>
        ///     Is selected channel Twitch Subscription Partner?
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool IsSubscriptionPartner(string channel)
        {
            return GetSingleSubscribedChannel(channel).Status != 422;
        }

        #endregion

        #region UserNode

        /// <summary>
        ///  Get your User Informations
        /// </summary>
        /// <returns></returns>
        public Templates.v3.User GetMyUserData()
        {
            var req = GetAuthenticatedSubmitRequest("user", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);
            var resp = Client.Execute<Templates.v3.User>(req);

            return resp.Data;
        }

        /// <summary>
        ///     Get Blocked Users
        /// </summary>
        /// <returns></returns>
        public Templates.v3.TwitchList<Templates.v3.Block> GetMyBlocks()
        {
            var req = GetAuthenticatedSubmitRequest("users/{user}/blocks", Method.GET);
            req.AddUrlSegment("user", userName);
            RestAPIVersion(Enums.APIVersion.v3);

            if(!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Block>>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///  Block user in your Channel
        /// </summary>
        /// <param name="userToBlock"></param>
        /// <returns></returns>
        public Templates.v3.TwitchDefaultResponse BlockUser(string userToBlock)
        {
            var req = GetAuthenticatedSubmitRequest("users/{user}/blocks/{target}", Method.PUT);
            req.AddUrlSegment("user", userName);
            req.AddUrlSegment("target", userToBlock);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToBlock))
            {
                var resp = Client.Execute<Templates.v3.TwitchDefaultResponse>(req);
                return resp.Data;
            }
            else
            {
                if(!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToBlock))
                    throw new TwitchException("UserToBlock and UserName not set", Enums.TwitchExceptionType.Authenticated, 3);
                else if(!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userToBlock))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if(string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToBlock))
                    throw new TwitchException("UserToBlock not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        /// <summary>
        ///  Unblock User from your Channel
        /// </summary>
        /// <param name="userToUnBlock"></param>
        /// <returns></returns>
        public Templates.v3.TwitchDefaultResponse UnBlockUser(string userToUnBlock)
        {
            var req = GetAuthenticatedSubmitRequest("users/{user}/blocks/{target}", Method.DELETE);
            req.AddUrlSegment("user", userName);
            req.AddUrlSegment("target", userToUnBlock);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToUnBlock))
            {
                var resp = Client.Execute<Templates.v3.TwitchDefaultResponse>(req);
                return resp.Data;
            }
            else
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToUnBlock))
                    throw new TwitchException("UserToUnBlock and UserName not set", Enums.TwitchExceptionType.Authenticated, 3);
                else if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userToUnBlock))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userToUnBlock))
                    throw new TwitchException("UserToUnBlock not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        /// <summary>
        ///  Follow to Selected Channel
        /// </summary>
        /// <param name="targetChannel"></param>
        /// <param name="notifications"></param>
        /// <returns></returns>
        public Templates.v3.FollowedChannel Follow(string targetChannel, bool notifications = false)
        {
            var req = GetAuthenticatedSubmitRequest("users/{user}/follows/channels/{target}", Method.PUT);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("target", targetChannel);
            req.AddParameter("notifications", notifications);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.FollowedChannel>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }


        /// <summary>
        ///  Unfollow to Selected Channel
        /// </summary>
        /// <param name="targetChannel"></param>
        /// <returns></returns>
        public Templates.v3.TwitchDefaultResponse UnFollow(string targetChannel)
        {
            var req = GetAuthenticatedSubmitRequest("users/{user}/follows/channels/{target}", Method.DELETE);
            req.AddUrlSegment("channel", userName);
            req.AddUrlSegment("target", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(targetChannel))
            {
                var resp = Client.Execute<Templates.v3.TwitchDefaultResponse>(req);
                return resp.Data;
            }
            else
            {
                if(string.IsNullOrEmpty(userName))
                    throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
                else if(string.IsNullOrEmpty(targetChannel))
                    throw new TwitchException("Target Channel not set", Enums.TwitchExceptionType.Authenticated, 2);
                else
                    throw new TwitchException("Unknown Error", Enums.TwitchExceptionType.Authenticated, 999);
            }
        }

        #endregion

        #region StreamNode

        /// <summary>
        ///  Get your Stream Informations
        /// </summary>
        /// <returns></returns>
        public Templates.v3.StreamAPIResult GetMyStream()
        {
            var req = GetAuthenticatedSubmitRequest("streams/{channel}", Method.GET);
            req.AddUrlSegment("channel", userName);
            RestAPIVersion(Enums.APIVersion.v3);

            if (!string.IsNullOrEmpty(userName))
            {
                var resp = Client.Execute<Templates.v3.StreamAPIResult>(req);
                return resp.Data;
            }
            else
            {
                throw new TwitchException("UserName not set", Enums.TwitchExceptionType.Authenticated, 1);
            }
        }

        /// <summary>
        ///     Get your Followed streams
        /// </summary>
        /// <returns></returns>
        //TODO: Sorting and Paging
        public Templates.v3.TwitchList<Templates.v3.Stream> GetFollowedStreams()
        {
            var req = GetAuthenticatedSubmitRequest("streams/followed", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);
            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Stream>>(req);

            return resp.Data;
        }
        #endregion

        #region VideoNode
        /// <summary>
        ///     Get Video Informations of your followed streams
        /// </summary>
        /// <returns></returns>
        //TODO Paging and Sorting
        public Templates.v3.TwitchList<Templates.v3.Video> GetFollowedVideos()
        {
            var req = GetSubmitRequest("videos/followed", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);
            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Video>>(req);

            return resp.Data;
        }
        #endregion

        #endregion

        #region Request Helper
        public RestRequest GetAuthenticatedSubmitRequest(string url, Method method)
        {
            RestRequest req = new RestRequest(url, method);
            req.AddHeader("Client-ID", twitchApp.clientID);
            req.AddHeader("Authorization", String.Format("OAuth {0}", oauthToken));

            return req;
        }
        #endregion
    }
}