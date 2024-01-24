using Android.Util;
using Firebase.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#nullable enable

namespace R3ELeaderboardViewer.Firebase
{
    public class CookieAwareWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; set; }
        public Uri Uri { get; set; }

        public CookieAwareWebClient() : this(new CookieContainer())
        {
        }

        public CookieAwareWebClient(CookieContainer cookies)
        {
            CookieContainer = cookies;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = CookieContainer;
            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return httpRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            string setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

            //do something if needed to parse out the cookie.
            if (setCookieHeader != null)
            {
                CookieContainer.SetCookies(request.RequestUri, setCookieHeader);
            }

            return response;
        }
    }


    public class UserData
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(UserData).Name;

        public static readonly DateTime SHOW_RECENT_COMPETITIONS_SINCE = DateTime.Now.AddMonths(-1);

        public static readonly string RACEROOM_URL = "https://game.raceroom.com/";
        public static readonly string USER_URL = RACEROOM_URL + "users/{0}";
        public static readonly string USER_INFO_URL = RACEROOM_URL + "utils/user-info/{0}";
        public static readonly string NEWS_FEED_URL = RACEROOM_URL + "widgets/newsfeed";

        private static readonly string CSRF_REGEX = @"csrf_token&quot;:&quot;([a-zA-Z0-9]+)&quot";
        private static readonly string UID_REGEX = @"user_id&quot;:(\d+),&quot";

        static UserData()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public FirebaseUser? FirebaseUser { get; internal set; } = null;
        public string? RaceRoomUsername { get; private set; }
        public List<Leaderboard> SavedLeaderboards { get; private set; } = new List<Leaderboard>();
        public List<Leaderboard> RecentCompetitions { get; private set; } = new List<Leaderboard>();
        public UserData(FirebaseUser firebaseUser)
        {
            FirebaseUser = firebaseUser;
        }

        public IDocumentReference GetReference()
        {
            return CrossCloudFirestore.Current.Instance.Collection("Users").Document(FirebaseUser.Uid);
        }

        public async Task<bool> Load()
        {
            await Task.Delay(200); // make sure user is loaded
            try
            {
                var user = await GetReference().GetAsync();
                if (!user.Exists)
                {
                    await Save();
                    return false;
                }
                
                await LoadSavedLeaderboards(await user.Reference.Collection("SavedLeaderboards").GetAsync());
                await LoadRecentCompetitions(await user.Reference.Collection("RecentCompetitions").GetAsync());

                await LoadRaceRoomUsername(user.Data);

                Log.Debug(TAG, "Loaded user data");

                return true;
            }
            catch (CloudFirestoreException e)
            {
                Log.Error(TAG, "Error loading user data " + e);
                return false;
            }
        }

        public async Task Save()
        {
            await GetReference().SetAsync(new Dictionary<string, object?>()
            {
                { "RaceRoomUsername", RaceRoomUsername },
            }, true);
        }

        public async Task LoadRaceRoomUsername(IDictionary<string, object?>? Data)
        {
            if (Data == null)
            {
                RaceRoomUsername = null;
                return;
            }
            if (Data.TryGetValue("RaceRoomUsername", out var raceRoomUsername))
            {
                var newUsername = (string?)raceRoomUsername;
                if (RaceRoomUsername == newUsername)
                {
                    return;
                }
                RaceRoomUsername = (string?)raceRoomUsername;
                await LoadRaceRoomUser(null);
            }
            else
            {
                RaceRoomUsername = null;
            }
        }

        public async Task<bool> SaveRaceRoomUsername(string raceRoomUsername)
        {
            var id = await ValidateRaceRoomUsername(raceRoomUsername);
            if (id == null && raceRoomUsername != null)
            {
                return false;
            }
            var userDataReference = GetReference();
            if (RaceRoomUsername != raceRoomUsername) { 

                RaceRoomUsername = raceRoomUsername;

                try
                {
                    await userDataReference.SetAsync(new Dictionary<string, object?>() {
                        { "RaceRoomUsername", raceRoomUsername },
                    }, true);
                }
                catch (CloudFirestoreException e)
                {
                    Log.Error(TAG, "Error saving RaceRoomUsername " + e);
                    return false;
                }
                

                _ = LoadRaceRoomUser(id);
                return raceRoomUsername != null;
            }
            return false;
        }


        private static string? ParseCsrfTokenFromResponse(string response)
        {
            var match = Regex.Match(response, CSRF_REGEX);
            if (!match.Success)
            {
                Log.Error(TAG, "Error parsing csrf token");
                return null;
            }
            return match?.Groups?[1]?.Value;
        }
        private static int? ParseUidFromResponse(string response)
        {
            var match = Regex.Match(response, UID_REGEX);
            if (!match.Success)
            {
                Log.Error(TAG, "Error parsing csrf token");
                return null;
            }
            var res = match?.Groups?[1]?.Value;
            return Utils.ParseInt(res);
        }

        private static async Task<Tuple<string?, int?>> GetCsrfTokenAndUid(string username)
        {
            // not loading from preferences because we're fetching it from the server anyways (when fetching the uid)
            string? token = null;

            using WebClient wc = new CookieAwareWebClient(cookies);
            try
            {
                // fetch csrf token
                var response = await wc.DownloadStringTaskAsync(string.Format(USER_URL, username));

                int? uid = ParseUidFromResponse(response);
                token = ParseCsrfTokenFromResponse(response);
                
                return new Tuple<string?, int?>(token, uid);
            }
            catch (WebException e)
            {
                Log.Error(TAG, "Error fetching csrf token " + e);
                return new Tuple<string?, int?> (null, null);
            }
        }

        private static CookieContainer cookies = new CookieContainer();
        private static async Task<string?> GetCsrfToken()
        {
            using WebClient wc = new CookieAwareWebClient(cookies);
            try
            {
                // fetch csrf token
                var response = await wc.DownloadStringTaskAsync(RACEROOM_URL);

                var token = ParseCsrfTokenFromResponse(response);

                return token;
            }
            catch (WebException e)
            {
                Log.Error(TAG, "Error fetching csrf token " + e);
                return null;
            }
        }

        private async Task LoadRaceRoomUser(int? userId)
        {
            if (RaceRoomUsername == null)
            {
                return;
            }

            using WebClient wc = new CookieAwareWebClient(cookies);
            try
            {
                string? token = null;
                if (userId == null)
                {
                    var tokenAndUid = await GetCsrfTokenAndUid(RaceRoomUsername);
                    token = tokenAndUid.Item1;
                    userId = tokenAndUid.Item2;
                }
                else
                {
                    token = await GetCsrfToken();
                }

                if (token == null || userId == null)
                {
                    return;
                }

                Log.Debug(TAG, "Uid: " + userId);
                Log.Debug(TAG, "CSRF Token: " + token);
                Utils.SetRequestedWith(wc);
                wc.Headers.Set("X-Csrf-Token", token);
                wc.Headers.Set(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded;charset=UTF-8");
                var response = await wc.UploadStringTaskAsync(string.Format(NEWS_FEED_URL, RaceRoomUsername), "POST", $"user_id={userId}&csrf_token={token}");
                JObject? json = null;
                try {
                    json = JsonConvert.DeserializeObject<JObject>(response);
                }
                catch (JsonReaderException e)
                {
                    Log.Error(TAG, "Error parsing user data " + e);
                    return;
                }
                if (json == null || json.ContainsKey("error"))
                {
                    Log.Error(TAG, "Error fetching user data " + json?["error"]);
                    return;
                }

                var data = json["context"]?["c"]?["newsfeed"]?["items"] as JArray;
                data ??= new JArray();
                var items = new List<NewsFeedItem>();
                foreach (var item in data)
                {
                    try
                    {
                        NewsFeedItem newsFeedItem = new NewsFeedItem(item as JObject);
                        items.Add(newsFeedItem);
                    }
                    catch (JsonReaderException e)
                    {
                        Log.Error(TAG, "Error parsing user data " + e);
                    }
                }
                items = items.Where(item => item.type.StartsWith("user.competition") && item.date >= SHOW_RECENT_COMPETITIONS_SINCE).ToList();

                Log.Debug(TAG, "Found " + RecentCompetitions.Count + " competitions");
                var expiration = RecentCompetitions.GroupBy(competition => competition.AddedAt.ToDateTime() < SHOW_RECENT_COMPETITIONS_SINCE);
                Log.Debug(TAG, "Found " + expiration.Count() + " groups of competitions");
                var newComps = new List<Leaderboard>();
                foreach (var group in expiration)
                {
                    Log.Debug(TAG, "Found " + group.Count() + " competitions " + (group.Key ? "older" : "newer") + " than " + SHOW_RECENT_COMPETITIONS_SINCE);
                    if (group.Key)
                    {
                        foreach (var competition in group)
                        {
                            Log.Debug(TAG, "Deleting expired recent competition " + competition);
                            await competition.Reference.DeleteAsync();
                        }
                    }
                    else
                    {
                        newComps.AddRange(group);
                    }
                }
                RecentCompetitions = newComps;

                // cross reference with saved leaderboards (exclude competitions that are already saved)
                var newCompetitions = new List<Leaderboard>();
                foreach (var item in items)
                {
                    var _id = Utils.ParseInt(item.related.path.Split("competitions/")[1].Split("/")[0]);
                    if (_id == null)
                    {
                        continue;
                    }
                    int id = (int)_id;
                    if (SavedLeaderboards.Any(leaderboard => leaderboard.LeaderboardId == id) || RecentCompetitions.Any(leaderboard => leaderboard.LeaderboardId == id))
                    {
                        continue;
                    }
                    newCompetitions.Add(new Leaderboard(
                        GetReference().Collection("RecentCompetitions"),
                        id,
                        null, null, null,
                        item.related.name,
                        "",
                        LeaderboardType.Competition,
                        new Timestamp((DateTimeOffset)item.date),
                        new List<LeaderboardSnapshot>()
                        ));
                }
                RecentCompetitions.AddRange(newCompetitions);
                Log.Debug(TAG, "Found " + newCompetitions.Count + " new competitions");

                if (newCompetitions.Count > 0)
                {
                    await SaveRecentCompetitions(newCompetitions, (int)userId);
                }
                await FetchSnapshotsIfNeeded((int)userId, RecentCompetitions);
            }
            catch (WebException e)
            {
                Log.Error(TAG, "Error fetching user data " + e);
            }
        }

#nullable disable
        private struct NewsFeedItem
        {
            public readonly DateTime date;
            public readonly string id;
            public readonly string type;
            public readonly NewsFeedItemSource related;
            public readonly NewsFeedItemSource source;

            public NewsFeedItem(JObject item)
            {
                if (!(
                    item.ContainsKey("date") &&
                    item.ContainsKey("id") &&
                    item.ContainsKey("type") &&
                    item.ContainsKey("related") &&
                    item.ContainsKey("source")
                    ))
                {
                    throw new JsonReaderException("Invalid NewsFeedItem");
                }
                DateTime.TryParseExact((string)item["date"], "d MMM yyyy HH:mm:ss", null, DateTimeStyles.None, out date);
                id = (string)item["id"];
                type = (string)item["type"];
                related = new NewsFeedItemSource(item["related"] as JObject);
                source = new NewsFeedItemSource(item["source"] as JObject);
            }

            public override string ToString() => $"NewsFeedItem(id={id}, type={type}, date={date}, related={related}, source={source})";
        }
        private readonly struct NewsFeedItemSource
        {
            public readonly string image;
            public readonly string name;
            public readonly string path;
            public readonly string type;

            public NewsFeedItemSource(JObject source)
            {
                if (!(
                    source.ContainsKey("image") &&
                    source.ContainsKey("name") &&
                    source.ContainsKey("path") &&
                    source.ContainsKey("type")
                    ))
                {
                    throw new JsonReaderException("Invalid NewsFeedItemSource");
                }
                image = (string)source["image"];
                name = (string)source["name"];
                path = (string)source["path"];
                type = (string)source["type"];
            }
        }
#nullable enable


        public async Task<int?> ValidateRaceRoomUsername(string raceRoomUsername)
        {
            if (raceRoomUsername == null)
            {
                return null;
            }
            raceRoomUsername = Uri.EscapeDataString(raceRoomUsername);
            using WebClient wc = new WebClient();
            try
            {
                Log.Debug(TAG, "Validating RaceRoomUsername " + raceRoomUsername);
                var response = await wc.DownloadStringTaskAsync(string.Format(USER_INFO_URL, raceRoomUsername));
                try
                {
                    var json = JsonConvert.DeserializeObject<JObject>(response);
                    if (json == null || json.ContainsKey("error"))
                    {
                        return null;
                    }
                    return (int?)json["id"];
                }
                catch (JsonReaderException)
                {
                    Log.Debug(TAG, "Invalid RaceRoomUsername " + raceRoomUsername);
                    return null;
                }
            }
            catch (WebException e)
            {
                Log.Error(TAG, "Error validating RaceRoomUsername " + e);
                return null;
            }
        }


        private async Task LoadSavedLeaderboards(IQuerySnapshot savedLeaderboards)
        {
            if (savedLeaderboards == null)
            {
                return; 
            }
            SavedLeaderboards = new List<Leaderboard>();
            foreach (var savedLeaderboard in savedLeaderboards.Documents)
            {
                var leaderboard = await Leaderboard.WithSnapshots(savedLeaderboard);
                if (leaderboard == null)
                {
                    await savedLeaderboard.Reference.DeleteAsync();
                }
                SavedLeaderboards.Add(leaderboard!);
            }
            Log.Debug(TAG, "Loaded " + SavedLeaderboards.Count + " saved leaderboards");
        }

        private async Task LoadRecentCompetitions(IQuerySnapshot recentCompetitions)
        {
            if (recentCompetitions == null)
            {
                return;
            }
            RecentCompetitions = new List<Leaderboard>();
            foreach (var recentCompetition in recentCompetitions.Documents)
            {
                var leaderboard = await Leaderboard.WithSnapshots(recentCompetition);
                if (leaderboard == null)
                {
                    await recentCompetition.Reference.DeleteAsync();
                }
                RecentCompetitions.Add(leaderboard!);
            }
            Log.Debug(TAG, "Loaded " + RecentCompetitions.Count + " recent competitions");
        }

        private async Task SaveRecentCompetitions(IEnumerable<Leaderboard>? competitions, int uid)
        {
            Log.Debug(TAG, "Saving recent competitions");
            foreach (var competition in competitions ?? RecentCompetitions)
            {
                await competition.Save();
                await competition.FetchSnapshot(uid);
            }
        }

        private async Task FetchSnapshotsIfNeeded(int userId, IEnumerable<Leaderboard>? leaderboards)
        {
            foreach (var leaderboard in leaderboards ?? SavedLeaderboards)
            {
                await leaderboard.FetchSnapshotIfNeeded(userId);
            }
        }
    }


    public class FirestoreLoadingException : Exception
    {
        public FirestoreLoadingException(string message) : base(message)
        {
        }
    }

    public class Leaderboard
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(Leaderboard).Name;

        public static readonly string LEADERBOARD_LISTING_URL = UserData.RACEROOM_URL + "leaderboard/listing/{0}";
        public static readonly DateTimeOffset SAVE_SNAPSHOT_EVERY = DateTimeOffset.Now.AddDays(-1);
        public static readonly int MAX_SNAPSHOTS = 10;

        public readonly int LeaderboardId;
        public readonly int? CarId;
        public readonly int? ClassId;
        public readonly int? TrackId;
        public readonly string LeaderboardName;
        public readonly string LeaderboardDescription;
        public readonly LeaderboardType Type;
        public readonly Timestamp AddedAt;

        public readonly ICollectionReference Collection;

        public string FirebaseId { get
            {
                return $"{Type}-{LeaderboardId}" + (Type == LeaderboardType.Leaderboard ? $"-{CarId ?? ClassId}-{TrackId}" : "");
            }
        }

        public List<LeaderboardSnapshot> Snapshots { get; internal set; } = new List<LeaderboardSnapshot>();

        public Leaderboard(IDocumentSnapshot savedLeaderboard)
        {
            Collection = savedLeaderboard.Reference.Parent;
            IDictionary<string, object> data = savedLeaderboard.Data!;
            
            var leaderboardId = Utils.GetIntOrDefault(data, "LeaderboardId");
            if (leaderboardId == null)
            {
                var idSplit = savedLeaderboard.Id.Split("-");
                if (idSplit.Length >= 2)
                {
                    leaderboardId = Utils.ParseInt(idSplit[1]);
                }
            }
            if (leaderboardId == null)
            {
                throw new FirestoreLoadingException("Invalid leaderboard id");
            }
            LeaderboardId = (int)leaderboardId;
            CarId = Utils.GetIntOrDefault(data, "CarId");
            ClassId = Utils.GetIntOrDefault(data, "ClassId");
            TrackId = Utils.GetIntOrDefault(data, "TrackId");
            LeaderboardName = (string)Utils.GetOrDefault(data, "LeaderboardName", "");
            LeaderboardDescription = (string)Utils.GetOrDefault(data, "LeaderboardDescription", "");
            Type = (LeaderboardType)(long)Utils.GetOrDefault(data, "Type", "");
            AddedAt = (Timestamp)Utils.GetOrDefault(data, "AddedAt", new Timestamp(DateTime.Now));

            if (FirebaseId != savedLeaderboard.Id)
            {
                Log.Warn(TAG, "FirebaseId mismatch " + FirebaseId + " != " + savedLeaderboard.Id);
            }


            if (CarId != null && ClassId != null)
            {
                Log.Warn(TAG, "SavedLeaderboard cannot have both CarId and ClassId");
            }
        }


        public IDocumentReference Reference
        {
            get
            {
                return Collection.Document(FirebaseId);
            }
        }

        public async Task LoadSnapshots(IDocumentReference savedLeaderboard)
        {
            Snapshots = new List<LeaderboardSnapshot>();
            var snapshots = await savedLeaderboard.Collection("Snapshots").GetAsync();
            foreach (var snapshot in snapshots.Documents)
            {
                Snapshots.Add(new LeaderboardSnapshot(snapshot, this));
            }
        }

        public static async Task<Leaderboard?> WithSnapshots(IDocumentSnapshot savedLeaderboard)
        {
            try
            {
                var leaderboard = new Leaderboard(savedLeaderboard);
                await leaderboard.LoadSnapshots(savedLeaderboard.Reference);
                return leaderboard;
            }
            catch (FirestoreLoadingException e)
            {
                Log.Error(TAG, "Error loading SavedLeaderboard " + e);
                return null;
            }
            
        }

        public Leaderboard(ICollectionReference collection, int leaderboardId, int? carId, int? classId, int? trackId, string leaderboardName, string leaderboardDescription, LeaderboardType leaderboardType, Timestamp addedAt, List<LeaderboardSnapshot> snapshots)
        {
            Collection = collection;
            LeaderboardId = leaderboardId;
            CarId = carId;
            ClassId = classId;
            TrackId = trackId;
            LeaderboardName = leaderboardName;
            LeaderboardDescription = leaderboardDescription;
            Type = leaderboardType;
            AddedAt = addedAt;
            Snapshots = snapshots;
        }

        public async Task Save()
        {
            await Reference.SetAsync(new Dictionary<string, object?>()
            {
                { "LeaderboardId", LeaderboardId },
                { "CarId", CarId },
                { "ClassId", ClassId },
                { "TrackId", TrackId },
                { "LeaderboardName", LeaderboardName },
                { "LeaderboardDescription", LeaderboardDescription },
                { "Type", (long)Type },
                { "AddedAt", AddedAt },
            }, true);

            var snapshotsCollection = Reference.Collection("Snapshots");
            foreach (var snapshot in Snapshots)
            {
                await snapshot.Save(snapshotsCollection.Document(snapshot.Timestamp.ToString()));
            }
        }


        private async Task<bool> FetchSnapshot(List<LeaderboardEntry> entries, int start, int count)
        {
            using WebClient wc = new WebClient();
            try
            {
                string url = string.Format(LEADERBOARD_LISTING_URL, LeaderboardId);
                
                var queryParams = new Dictionary<string, string>()
                {
                    { "start", start.ToString() },
                    { "count", count.ToString() },
                };
                if (CarId != null)
                {
                    queryParams.Add("car_class", CarId.ToString());
                }
                else if (ClassId != null)
                {
                    queryParams.Add("car_class", ClassId.ToString());
                }
                if (TrackId != null)
                {
                    queryParams.Add("track", TrackId.ToString());
                }
                if (queryParams.Count > 0)
                {
                    url += "?" + string.Join("&", queryParams.Select(kvp => kvp.Key + "=" + kvp.Value));
                }

                Log.Debug(TAG, "Fetching leaderboard " + url);

                Utils.SetRequestedWith(wc);
                var response = await wc.DownloadStringTaskAsync(url);
                JObject? json = null;
                try
                {
                    json = JsonConvert.DeserializeObject<JObject>(response);
                }
                catch { }
                if (json == null || json.ContainsKey("error"))
                {
                    Log.Error(TAG, "Error fetching leaderboard " + json?["error"]);
                    return false;
                }
                var data = json["context"]?["c"]?["results"] as JArray;
                data ??= new JArray();
                foreach (var entry in data)
                {
                    try
                    {
                        var dateTimeString = (string)entry["date_time"];
                        if (!DateTime.TryParseExact(dateTimeString, dateTimeString.Contains('T') ? "yyyy-MM-ddTHH:mm:ss" : "MM/dd/yyyy HH:mm:ss", null, DateTimeStyles.None, out DateTime timestamp))
                        {
                            Log.Error(TAG, "Error parsing timestamp from " + dateTimeString);
                            continue;
                        }


                        LeaderboardEntry leaderboardEntry = new LeaderboardEntry((string)entry["country"]["code"], (int)entry["global_index"], (string)entry["driver"]["avatar"], Utils.ParseInt(((string)entry["driver"]["path"]).Split("/")[^2]) ?? -1, (string)entry["driver"]["name"], Utils.ParseRaceRoomLaptime(((string)entry["laptime"]).Split(',')[0]), Utils.ParseRaceRoomLaptime((string)entry["relative_laptime"], null), (string)entry["car_class"]["car"]["icon"], (string)entry["track"]["image"], Utils.ParseDifficulty((string)entry["driving_model"]), (string)entry["team"], (double?)entry["champ_points"], new Timestamp(timestamp));
                        entries.Add(leaderboardEntry);
                    }
                    catch (JsonReaderException e)
                    {
                        Log.Error(TAG, "Error parsing leaderboard entry " + e);
                    }
                }

                return true;
            }
            catch (WebException e)
            {
                Log.Error(TAG, "Error fetching leaderboard " + e);
            }
            return false;
        }

        public static readonly int FETCH_BATCH_SIZE = 200;
        public static readonly int MINIMUM_SPACE_AHEAD = 3;
        public static readonly int MINIMUM_SPACE_BEHIND = 7;
        public async Task<LeaderboardSnapshot?> FetchSnapshot(int? uid)
        {
            var start = 0;
            int userIndex = -1;
            var entries = new List<LeaderboardEntry>();
            bool res = true;
            LeaderboardSnapshot snap = new LeaderboardSnapshot(new Timestamp(DateTime.Now), entries, this); ;
            do
            {
                res = await FetchSnapshot(entries, start, FETCH_BATCH_SIZE);
                if (res && uid != null)
                {
                    userIndex = entries.FindIndex(x => x.Uid == uid);
                    if (userIndex != -1)
                    {
                        snap.UserIndex = userIndex;
                    }
                }
                start += FETCH_BATCH_SIZE;
            }
            while (userIndex != -1 && entries.Count - userIndex < MINIMUM_SPACE_BEHIND && res);

            if (userIndex == -1)
            {
                Log.Warn(TAG, "User not found in leaderboard " + ToString());
            }
            Snapshots.Add(snap);
            await SaveSnapshot(snap);

            return snap;
        }

        public async Task<LeaderboardSnapshot?> FetchSnapshotIfNeeded(int uid)
        {
            LeaderboardSnapshot? snap = null;
            if (Snapshots.Count == 0)
            {
                snap = await FetchSnapshot(uid);
            }
            else
            {
                var lastSnapshot = Snapshots[^1];
                if (lastSnapshot.Timestamp.ToDateTime() < SAVE_SNAPSHOT_EVERY)
                {
                    snap = await FetchSnapshot(uid);

                    if (Snapshots.Count > MAX_SNAPSHOTS)
                    {
                        var toRemove = Snapshots[0];
                        await RemoveSnapshot(toRemove);
                        Snapshots.RemoveAt(0);
                    }
                }
            }
            if (snap != null)
            {
                Snapshots.Add(snap);
                await SaveSnapshot(snap);
            }
            return snap;
        }

        public Task SaveSnapshot(LeaderboardSnapshot snapshot)
        {
            Log.Debug(TAG, $"Saving snapshot {snapshot.Reference.Path}/{snapshot}");
            return snapshot.Save();
        }

        public Task RemoveSnapshot(LeaderboardSnapshot snapshot)
        {
            return snapshot.Reference.DeleteAsync();
        }

        public override string ToString()
        {
            return $"SavedLeaderboard(LeaderboardId={LeaderboardId}, CarId={CarId}, ClassId={ClassId}, TrackId={TrackId}, LeaderboardName={LeaderboardName}, LeaderboardDescription={LeaderboardDescription}, Type={Type}, AddedAt={AddedAt.ToDateTime()}, Snapshots-{Snapshots.Count})";
        }
    }

    public class LeaderboardSnapshot
    {
        public readonly Timestamp Timestamp;
        public int? UserIndex { get; internal set; }
        public readonly List<LeaderboardEntry> Entries;

        public readonly Leaderboard Parent;
        public LeaderboardSnapshot(IDocumentSnapshot snapshot, Leaderboard parent)
        {
            var snapshotData = snapshot.Data;
            Timestamp = (Timestamp)Utils.GetOrDefault(snapshotData, "Timestamp", new Timestamp(DateTime.Now))!;
            UserIndex = Utils.GetIntOrDefault(snapshotData, "UserIndex");
            Entries = new List<LeaderboardEntry>();

            var entriesValue = Utils.GetOrDefault(snapshotData, "Entries", new List<object>())!;
            foreach (var entry in (List<object>)entriesValue)
            {
                var entryDict = (Dictionary<string, object>)entry;
                Entries.Add(new LeaderboardEntry(entryDict));
            }
            Parent = parent;
        }

        public LeaderboardSnapshot(Timestamp timestamp, List<LeaderboardEntry> entries, Leaderboard parent)
        {
            Timestamp = timestamp;
            Entries = entries;
            Parent = parent;
        }

        public Task Save(IDocumentReference doc)
        {
            return doc.SetAsync(new Dictionary<string, object?>()
            {
                { "Timestamp", Timestamp },
                { "UserIndex", UserIndex },
                { "Entries", Entries },
            }, true);
        }

        public Task Save()
        {
            return Save(Reference);
        }

        public IDocumentReference Reference
        {
            get
            {
                return Parent.Reference.Collection("Snapshots").Document(Timestamp.ToDateTimeOffset().ToUnixTimeMilliseconds().ToString());
            }
        }

        public override string ToString()
        {
            return $"LeaderboardSnapshot(Timestamp={Timestamp.ToDateTime()}, UserIndex={UserIndex}, Entries={Entries.Count})";
        }
    }

    public struct LeaderboardEntry
    {
        public readonly string CountryCode;
        public readonly int Position;
        public readonly string? ProfilePictureUrl;
        public readonly int? Uid;
        public readonly string DisplayName;
        public readonly double Laptime;
        public readonly double? Gap;
        public readonly string? LiveryImageUrl;
        public readonly string? TrackImageUrl;
        public readonly GameplayDifficulty Difficulty;
        public readonly string Team;
        public readonly double? Points;
        public readonly Timestamp Timestamp;

        public LeaderboardEntry(Dictionary<string, object> entry)
        {
            CountryCode = (string)Utils.GetOrDefault(entry, "CountryCode", "aq"); // aq is Antarctica, 'cause why not
            Position = Utils.GetIntOrDefault(entry, "Position", 0);
            ProfilePictureUrl = (string?)Utils.GetOrDefault(entry, "ProfilePictureUrl");
            Uid = Utils.GetIntOrDefault(entry, "Uid");
            DisplayName = (string)Utils.GetOrDefault(entry, "DisplayName", "N/A");
            Laptime = (double)Utils.GetOrDefault(entry, "Laptime", 0);
            Gap = (double?)Utils.GetOrDefault(entry, "Gap");
            LiveryImageUrl = (string?)Utils.GetOrDefault(entry, "LiveryImageUrl");
            TrackImageUrl = (string?)Utils.GetOrDefault(entry, "TrackImageUrl");
            Difficulty = (GameplayDifficulty)(long)Utils.GetOrDefault(entry, "Difficulty", -1);
            Team = (string)Utils.GetOrDefault(entry, "Team", "");
            Points = (double?)Utils.GetOrDefault(entry, "Points");
            Timestamp = (Timestamp)Utils.GetOrDefault(entry, "Timestamp", new Timestamp(DateTime.Now));
        }

        public LeaderboardEntry(string countryCode, int position, string profilePictureUrl, int uid, string displayName, double laptime, double? gap, string liveryImageUrl, string trackImageUrl, GameplayDifficulty difficulty, string team, double? points, Timestamp timestamp)
        {
            CountryCode = countryCode;
            Position = position;
            ProfilePictureUrl = profilePictureUrl;
            Uid = uid;
            DisplayName = displayName;
            Laptime = laptime;
            Gap = gap;
            LiveryImageUrl = liveryImageUrl;
            TrackImageUrl = trackImageUrl;
            Difficulty = difficulty;
            Team = team;
            Points = points;
            Timestamp = timestamp;
        }


        public override string ToString()
        {
            return $"LeaderboardEntry(CountryCode={CountryCode}, Position={Position}, ProfilePictureUrl={ProfilePictureUrl}, Uid={Uid}, DisplayName={DisplayName}, Laptime={Laptime}, Gap={Gap}, LiveryImageUrl={LiveryImageUrl}, TrackImageUrl={TrackImageUrl}, Difficulty={Difficulty}, Team={Team}, Points={Points}, Timestamp={Timestamp.ToDateTime()})";
        }
    }

    public enum GameplayDifficulty
    {
        Unknown = -1,
        GetReal = 0,
        Amateur = 1,
        Novice = 2,
    }

    public enum LeaderboardType
    {
        None = -1,
        Leaderboard = 0,
        Competition = 1,
    }
}