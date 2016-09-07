using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;
using Streamhelper.Twitch.API.Helper;


namespace Streamhelper.Twitch.API.Handler.Connecter
{
    public class Anonymous
    {
        public readonly RestClient Client;
        public readonly Parameter TwitchAPIHeader = new Parameter() { Name = "Accept", Type = ParameterType.HttpHeader, Value = "application/vnd.twitchtv.v3+json" };
        public readonly TwitchApplication twitchApp;


        public Anonymous(TwitchApplication appData) : this(Streamhelper.Twitch.API.Helper.TwitchAPIStructure.APIBaseUrl, appData)
        {

        }

        public Anonymous(String BaseUrl, TwitchApplication appData)
        {
            Client = new RestClient(BaseUrl);
            Client.AddHandler("application/json", new DynamicJsonDeserializer());
            Client.DefaultParameters.Add(TwitchAPIHeader);

            twitchApp = appData;
        }

        #region Async


        #region RootNode
        /// <summary>
        ///     Returns the API Root Object
        /// </summary>
        public Task<Templates.v3.RootResult> GetAPIRootAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.RootResult>();
            var req = GetSubmitRequest("/", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.RootResult>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;
        }

        #endregion

        #region ChannelsNode
        /// <summary>
        ///     Return a TwitchAPI version 3 Channel Object
        /// </summary>
        /// <param name="targetChannel">Target Channel as String</param>
        /// <returns></returns>
        public Task<Templates.v3.Channel> GetChannelAsync(string targetChannel)
        {
            var tcs = new TaskCompletionSource<Templates.v3.Channel>();
            var req = GetSubmitRequest("channels/{channel}", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.Channel>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;
        }

        /// <summary>
        ///     Get list of teams channel belongs to
        /// </summary>
        /// <param name="targetChannel"></param>
        public Task<Templates.v3.TwitchList<Templates.v3.Team>> GetTeamsAsync(string targetChannel)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Team>>();
            var req = GetSubmitRequest("channels/{channel}/teams", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Team>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;
        }

        #endregion

        #region ChatNode

        /// <summary>
        ///     Get list of every emoticon object
        /// </summary>
        public Task<Templates.v3.TwitchList<Templates.v3.Emoticon>> GetEmoticonsAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Emoticon>>();
            var req = GetSubmitRequest("chat/emoticons", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Emoticon>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;
        }

        /// <summary>
        ///     Get chat badges for channel
        /// </summary>
        /// <param name="targetChannel"></param>
        public Task<Templates.v3.BadgeResult> GetBadgesAsync(string targetChannel)
        {
            var tcs = new TaskCompletionSource<Templates.v3.BadgeResult>();
            var req = GetSubmitRequest("channels/{channel}/badges", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.BadgeResult>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });

            return tcs.Task;
        }
        #endregion

        #region FollowsNode

        //TODO Sort
        /// <summary>
        ///     Get channel's list of following users
        /// </summary>
        /// <param name="targetChannel"></param>
        public Task<Templates.v3.TwitchList<Templates.v3.Follower>> GetFollowerAsync(string targetChannel)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Follower>>();
            var req = GetSubmitRequest("channels/{channel}/follows", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Follower>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        //TODO Sort
        /// <summary>
        ///     Get a user's list of followed channels
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="sort"></param>
        /// <param name="sorttype"></param>
        /// <returns></returns>
        public Task<Templates.v3.TwitchList<Templates.v3.FollowedChannel>> GetFollowedChannelsAsync(string targetUser, Enums.APISorting sort, Enums.APISortingType sorttype = Enums.APISortingType.created_at)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.FollowedChannel>>();
            var req = GetSubmitRequest("users/{user}/follows/channels", Method.GET);
            req.AddUrlSegment("user", targetUser);
            req.AddParameter("direction", sort);
            req.AddParameter("sortby", sorttype);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.FollowedChannel>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        /// <summary>
        ///     Get status of follow relationship between user and target channel
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="targetChannel"></param>
        public Task<Templates.v3.FollowedChannel> GetSingleFollowedChannelAsync(string targetUser, string targetChannel)
        {
            var tcs = new TaskCompletionSource<Templates.v3.FollowedChannel>();
            var req = GetSubmitRequest("users/{user}/follows/channels/{target}", Method.GET);
            req.AddUrlSegment("user", targetUser);
            req.AddUrlSegment("target", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.FollowedChannel>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        /// <summary>
        ///     Check if User is following a channel
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="targetChannel"></param>
        /// <returns></returns>
        public async Task<bool> IsFollowingAsync(string targetUser, string targetChannel)
        {
            Templates.v3.FollowedChannel result = await GetSingleFollowedChannelAsync(targetUser, targetChannel);
            return result.Status != 404;
        }
        #endregion

        #region GamesNode
        /// <summary>
        ///     Get games by number of viewers
        /// </summary>
        public Task<Templates.v3.TwitchList<Templates.v3.TopGame>> GetTopGamesAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.TopGame>>();
            var req = GetSubmitRequest("games/top", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.TopGame>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }
        #endregion

        #region SearchNode
        //TODO Sorting
        /// <summary>
        ///     Find channels
        /// </summary>
        /// <param name="q"></param>
        public Task<Templates.v3.TwitchList<Templates.v3.Channel>> SearchChannelsAsync(string q)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Channel>>();
            var req = GetSubmitRequest("search/channels", Method.GET);
            req.AddParameter("q", q);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Channel>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        //TODO Sorting
        /// <summary>
        ///     Find Streams
        /// </summary>
        /// <param name="q"></param>
        /// <param name="hlsOnly"></param>
        public Task<Templates.v3.TwitchList<Templates.v3.Stream>> SearchStreamsAsync(string q, bool hlsOnly = false)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Stream>>();
            var req = GetSubmitRequest("search/streams", Method.GET);
            req.AddParameter("q", q);
            req.AddParameter("hls", hlsOnly);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Stream>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        //TODO Sorting
        /// <summary>
        ///     Find Games
        /// </summary>
        /// <param name="q"></param>
        /// <param name="isLive"></param>
        public Task<Templates.v3.TwitchList<Templates.v3.Game>> SearchGamesAsync(string q, bool isLive = false)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Game>>();
            var req = GetSubmitRequest("search/games", Method.GET);
            req.AddParameter("q", q);
            req.AddParameter("live", isLive);
            req.AddParameter("type", "suggest");
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Game>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }
        #endregion

        #region StreamsNode
        /// <summary>
        ///     Get stream object
        /// </summary>
        /// <param name="targetChannel"></param>
        /// <returns></returns>
        public Task<Templates.v3.StreamAPIResult> GetStreamAsync(string targetChannel)
        {
            var tcs = new TaskCompletionSource<Templates.v3.StreamAPIResult>();
            var req = GetSubmitRequest("streams/{channel}", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.StreamAPIResult>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }


       
        /// <summary>
        ///     Check if Channel is Live
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task<bool> IsLiveAsync(string channel)
        {
            Templates.v3.StreamAPIResult result = await GetStreamAsync(channel);
            return result.Stream != null;
        }



        //TODO Sorting
        /// <summary>
        ///     Get stream object
        /// </summary>
        /// <param name="game"></param>
        /// <param name="targetChannel"></param>
        /// <param name="clientId"></param>
        /// <param name="stream_type"></param>
        public Task<Templates.v3.TwitchList<Templates.v3.Stream>> GetStreamsAsync(string game = null, string targetChannel = null, string clientId = null, Enums.StreamType stream_type = Enums.StreamType.all)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Stream>>();
            var req = GetSubmitRequest("streams", Method.GET);
            if (!string.IsNullOrEmpty(game))
                req.AddParameter("game", game);
            if (!string.IsNullOrEmpty(targetChannel))
                req.AddParameter("channel", targetChannel);

            req.AddParameter("stream_type", stream_type);

            if (!string.IsNullOrEmpty(targetChannel))
                req.AddParameter("client_id", clientId);

            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Stream>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        //TODO Sorting
        /// <summary>
        ///     Get a list of featured streams
        /// </summary>
        public Task<Templates.v3.TwitchList<Templates.v3.Featured>> GetFeaturedStreamsAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Featured>>();
            var req = GetSubmitRequest("streams/featured", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Featured>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        /// <summary>
        ///     Get a summary of streams
        /// </summary>
        /// <param name="game"></param>
        public Task<Templates.v3.StreamSummary> GetStreamSummaryAsync(string game = null)
        {
            var tcs = new TaskCompletionSource<Templates.v3.StreamSummary>();
            var req = GetSubmitRequest("streams/summary", Method.GET);
            if (!string.IsNullOrEmpty(game))
                req.AddParameter("game", game);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.StreamSummary>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }
        #endregion

        #region TeamsNode

        // TODO Sort
        /// <summary>
        ///     Get list of active team objects
        /// </summary>
        public Task<Templates.v3.TwitchList<Templates.v3.Team>> GetTeamsAsync()
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Team>>();
            var req = GetSubmitRequest("team", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Team>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        /// <summary>
        ///     Get team object
        /// </summary>
        /// <param name="team"></param>
        public Task<Templates.v3.Team> GetTeamAsync(string team)
        {
            var tcs = new TaskCompletionSource<Templates.v3.Team>();
            var req = GetSubmitRequest("teams/{team}", Method.GET);
            req.AddUrlSegment("team", team);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.Team>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }
        #endregion

        #region UsersNode
        /// <summary>
        ///     Get user object
        /// </summary>
        /// <param name="targetUser"></param>
        public Task<Templates.v3.User> GetUserAsync(string targetUser)
        {
            var tcs = new TaskCompletionSource<Templates.v3.User>();
            var req = GetSubmitRequest("users/{user}", Method.GET);
            req.AddUrlSegment("user", targetUser);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.User>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }
        #endregion

        #region VideoNode
        /// <summary>
        ///     Get video object
        /// </summary>
        /// <param name="id"></param>
        public Task<Templates.v3.Video> GetVideoAsync(string id)
        {
            var tcs = new TaskCompletionSource<Templates.v3.Video>();
            var req = GetSubmitRequest("videos/{id}", Method.GET);
            req.AddUrlSegment("id", id);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.Video>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }


        // TODO Sort
        /// <summary>
        ///     Get top videos by number of views
        /// </summary>
        /// <param name="game"></param>
        /// <param name="period"></param>
        public Task<Templates.v3.TwitchList<Templates.v3.Video>> GetTopVideosAsync(string game = null, Enums.VideoPeriodType period = Enums.VideoPeriodType.week)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Video>>();
            var req = GetSubmitRequest("videos/top", Method.GET);
            if (!string.IsNullOrEmpty(game))
                req.AddParameter("game", game);
            req.AddParameter("period", period);
            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Video>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }

        // TODO Sort
        /// <summary>
        ///     Get list of video objects belonging to channel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="isBroadcast"></param>
        /// <param name="isHLS"></param>
        public Task<Templates.v3.TwitchList<Templates.v3.Video>> GetChannelVideosAsync(string channel, bool isBroadcast, bool isHLS)
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Video>>();
            var req = GetSubmitRequest("channels/{channel}/videos", Method.GET);
            req.AddUrlSegment("channel", channel);
            req.AddParameter("broadcasts", isBroadcast);
            req.AddParameter("hls", isHLS);

            RestAPIVersion(Enums.APIVersion.v3);

            Client.ExecuteAsync<Templates.v3.TwitchList<Templates.v3.Video>>(req, (response) =>
            {
                tcs.SetResult(response.Data);
            });
            return tcs.Task;
        }
        #endregion


        #endregion



        #region NonAsync


        #region RootNode
        /// <summary>
        ///     Returns the API Root Object
        /// </summary>
        public Templates.v3.RootResult GetAPIRoot()
        {
            var req = GetSubmitRequest("/", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.RootResult>(req);

            return resp.Data;
        }

        #endregion

        #region ChannelsNode
        /// <summary>
        ///     Return a TwitchAPI version 3 Channel Object
        /// </summary>
        /// <param name="targetChannel">Target Channel as String</param>
        /// <returns></returns>
        public Templates.v3.Channel GetChannel(string targetChannel)
        {

            var req = GetSubmitRequest("channels/{channel}", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.Channel>(req);

            return resp.Data;
        }

        /// <summary>
        ///     Get list of teams channel belongs to
        /// </summary>
        /// <param name="targetChannel"></param>
        public Templates.v3.TwitchList<Templates.v3.Team> GetTeams(string targetChannel)
        {
            var req = GetSubmitRequest("channels/{channel}/teams", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Team>>(req);

            return resp.Data;
        }

        #endregion

        #region ChatNode

        /// <summary>
        ///     Get list of every emoticon object
        /// </summary>
        public Templates.v3.TwitchList<Templates.v3.Emoticon> GetEmoticons()
        {
            var req = GetSubmitRequest("chat/emoticons", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Emoticon>>(req);

            return resp.Data;
        }

        /// <summary>
        ///     Get chat badges for channel
        /// </summary>
        /// <param name="targetChannel"></param>
        public Templates.v3.BadgeResult GetBadges(string targetChannel)
        {
            var req = GetSubmitRequest("channels/{channel}/badges", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.BadgeResult>(req);

            return resp.Data;
        }
        #endregion

        #region FollowsNode

        //TODO Sort
        /// <summary>
        ///     Get channel's list of following users
        /// </summary>
        /// <param name="targetChannel"></param>
        public Templates.v3.TwitchList<Templates.v3.Follower> GetFollower(string targetChannel)
        {
            var req = GetSubmitRequest("channels/{channel}/follows", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Follower>>(req);

            return resp.Data;
        }

        //TODO Sort
        /// <summary>
        ///     Get a user's list of followed channels
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="sort"></param>
        /// <param name="sorttype"></param>
        /// <returns></returns>
        public Templates.v3.TwitchList<Templates.v3.FollowedChannel> GetFollowedChannels(string targetUser, Enums.APISorting sort, Enums.APISortingType sorttype = Enums.APISortingType.created_at)
        {
            var req = GetSubmitRequest("users/{user}/follows/channels", Method.GET);
            req.AddUrlSegment("user", targetUser);
            req.AddParameter("direction", sort);
            req.AddParameter("sortby", sorttype);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.FollowedChannel>>(req);

            return resp.Data;
        }

        /// <summary>
        ///     Get status of follow relationship between user and target channel
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="targetChannel"></param>
        public Templates.v3.FollowedChannel GetSingleFollowedChannel(string targetUser, string targetChannel)
        {
            var req = GetSubmitRequest("users/{user}/follows/channels/{target}", Method.GET);
            req.AddUrlSegment("user", targetUser);
            req.AddUrlSegment("target", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.FollowedChannel>(req);

            return resp.Data;
        }
        /// <summary>
        ///     Check if User is following a channel
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="targetChannel"></param>
        /// <returns></returns>
        public bool IsFollowing(string targetUser, string targetChannel)
        {
            return GetSingleFollowedChannel(targetUser, targetChannel).Status != 404;
        }
        #endregion

        #region GamesNode
        /// <summary>
        ///     Get games by number of viewers
        /// </summary>
        public Templates.v3.TwitchList<Templates.v3.TopGame> GetTopGames()
        {
            var req = GetSubmitRequest("games/top", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.TopGame>>(req);

            return resp.Data;
        }
        #endregion

        #region SearchNode
        //TODO Sorting
        /// <summary>
        ///     Find channels
        /// </summary>
        /// <param name="q"></param>
        public Templates.v3.TwitchList<Templates.v3.Channel> SearchChannels(string q)
        {
            var req = GetSubmitRequest("search/channels", Method.GET);
            req.AddParameter("q", q);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Channel>>(req);

            return resp.Data;
        }

        //TODO Sorting
        /// <summary>
        ///     Find Streams
        /// </summary>
        /// <param name="q"></param>
        /// <param name="hlsOnly"></param>
        public Templates.v3.TwitchList<Templates.v3.Stream> SearchStreams(string q, bool hlsOnly = false)
        {
            var req = GetSubmitRequest("search/streams", Method.GET);
            req.AddParameter("q", q);
            req.AddParameter("hls", hlsOnly);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Stream>>(req);

            return resp.Data;
        }
        //TODO Sorting
        /// <summary>
        ///     Find Games
        /// </summary>
        /// <param name="q"></param>
        /// <param name="isLive"></param>
        public Templates.v3.TwitchList<Templates.v3.Game> SearchGames(string q, bool isLive = false)
        {

            var req = GetSubmitRequest("search/games", Method.GET);
            req.AddParameter("q", q);
            req.AddParameter("live", isLive);
            req.AddParameter("type", "suggest");
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Game>>(req);

            return resp.Data;
        }
        #endregion

        #region StreamsNode
        /// <summary>
        ///     Get stream object
        /// </summary>
        /// <param name="targetChannel"></param>
        /// <returns></returns>
        public Templates.v3.StreamAPIResult GetStream(string targetChannel)
        {
            var req = GetSubmitRequest("streams/{channel}", Method.GET);
            req.AddUrlSegment("channel", targetChannel);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.StreamAPIResult>(req);

            return resp.Data;
        }



        /// <summary>
        ///     Check if Channel is Live
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool IsLive(string channel)
        {
            return GetStream(channel).Stream != null;
        }



        //TODO Sorting
        /// <summary>
        ///     Get stream object
        /// </summary>
        /// <param name="game"></param>
        /// <param name="targetChannel"></param>
        /// <param name="clientId"></param>
        /// <param name="stream_type"></param>
        public Templates.v3.TwitchList<Templates.v3.Stream> GetStreams(string game = null, string targetChannel = null, string clientId = null, Enums.StreamType stream_type = Enums.StreamType.all)
        {

            var req = GetSubmitRequest("streams", Method.GET);
            if (!string.IsNullOrEmpty(game))
                req.AddParameter("game", game);
            if (!string.IsNullOrEmpty(targetChannel))
                req.AddParameter("channel", targetChannel);

            req.AddParameter("stream_type", stream_type);

            if (!string.IsNullOrEmpty(targetChannel))
                req.AddParameter("client_id", clientId);

            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Stream>>(req);

            return resp.Data;
        }

        //TODO Sorting
        /// <summary>
        ///     Get a list of featured streams
        /// </summary>
        public Templates.v3.TwitchList<Templates.v3.Featured> GetFeaturedStreams()
        {
            var req = GetSubmitRequest("streams/featured", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Featured>>(req);

            return resp.Data;
        }

        /// <summary>
        ///     Get a summary of streams
        /// </summary>
        /// <param name="game"></param>
        public Templates.v3.StreamSummary GetStreamSummary(string game = null)
        {

            var req = GetSubmitRequest("streams/summary", Method.GET);
            if (!string.IsNullOrEmpty(game))
                req.AddParameter("game", game);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.StreamSummary>(req);

            return resp.Data;
        }
        #endregion

        #region TeamsNode

        // TODO Sort
        /// <summary>
        ///     Get list of active team objects
        /// </summary>
        public Templates.v3.TwitchList<Templates.v3.Team> GetTeams()
        {
            var tcs = new TaskCompletionSource<Templates.v3.TwitchList<Templates.v3.Team>>();
            var req = GetSubmitRequest("team", Method.GET);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Team>>(req);

            return resp.Data;
        }

        /// <summary>
        ///     Get team object
        /// </summary>
        /// <param name="team"></param>
        public Templates.v3.Team GetTeam(string team)
        {
            var req = GetSubmitRequest("teams/{team}", Method.GET);
            req.AddUrlSegment("team", team);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.Team>(req);

            return resp.Data;
        }
        #endregion

        #region UsersNode
        /// <summary>
        ///     Get user object
        /// </summary>
        /// <param name="targetUser"></param>
        public Templates.v3.User GetUser(string targetUser)
        {
            var req = GetSubmitRequest("users/{user}", Method.GET);
            req.AddUrlSegment("user", targetUser);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.User>(req);

            return resp.Data;
        }
        #endregion

        #region VideoNode
        /// <summary>
        ///     Get video object
        /// </summary>
        /// <param name="id"></param>
        public Templates.v3.Video GetVideo(string id)
        {
            var req = GetSubmitRequest("videos/{id}", Method.GET);
            req.AddUrlSegment("id", id);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.Video>(req);

            return resp.Data;
        }


        // TODO Sort
        /// <summary>
        ///     Get top videos by number of views
        /// </summary>
        /// <param name="game"></param>
        /// <param name="period"></param>
        public Templates.v3.TwitchList<Templates.v3.Video> GetTopVideos(string game = null, Enums.VideoPeriodType period = Enums.VideoPeriodType.week)
        {
            var req = GetSubmitRequest("videos/top", Method.GET);
            if (!string.IsNullOrEmpty(game))
                req.AddParameter("game", game);
            req.AddParameter("period", period);
            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Video>>(req);

            return resp.Data;
        }

        // TODO Sort
        /// <summary>
        ///     Get list of video objects belonging to channel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="isBroadcast"></param>
        /// <param name="isHLS"></param>
        public Templates.v3.TwitchList<Templates.v3.Video> GetChannelVideos(string channel, bool isBroadcast, bool isHLS)
        {
            var req = GetSubmitRequest("channels/{channel}/videos", Method.GET);
            req.AddUrlSegment("channel", channel);
            req.AddParameter("broadcasts", isBroadcast);
            req.AddParameter("hls", isHLS);

            RestAPIVersion(Enums.APIVersion.v3);

            var resp = Client.Execute<Templates.v3.TwitchList<Templates.v3.Video>>(req);

            return resp.Data;
        }
        #endregion


        #endregion


        #region Request Helper
        public RestRequest GetSubmitRequest(string url, Method method)
        {
            RestRequest req = new RestRequest(url, method);
            req.AddHeader("Client-ID", this.twitchApp.clientID);
            return req;
        }

        public void RestAPIVersion(Enums.APIVersion version)
        {
            switch (version)
            {
                case Enums.APIVersion.v2:
                    TwitchAPIHeader.Value = "application/vnd.twitchtv.v2+json";
                    break;
                case Enums.APIVersion.v3:
                    TwitchAPIHeader.Value = "application/vnd.twitchtv.v3+json";
                    break;
                default:
                    TwitchAPIHeader.Value = "application/vnd.twitchtv.v3+json";
                    break;
            }
        }
        #endregion
    }
}