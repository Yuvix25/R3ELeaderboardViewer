using Android.Util;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using R3ELeaderboardViewer.Firebase;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Linq;
using Plugin.CloudFirestore;

#nullable enable

namespace R3ELeaderboardViewer.RaceRoom
{
    public class RaceRoomApiManager
    {
        public static readonly string TAG = "R3ELeaderboardViewer:" + typeof(RaceRoomApiManager).Name;

        public static readonly string RACEROOM_URL = "https://game.raceroom.com/";
        public static readonly string USER_URL = RACEROOM_URL + "users/{0}";
        public static readonly string USER_INFO_URL = RACEROOM_URL + "utils/user-info/{0}";
        public static readonly string COMPETITIONS_URL = RACEROOM_URL + "competitions?json";
        public static readonly string LEADERBOARD_LISTING_URL = RACEROOM_URL + "leaderboard/listing/{0}";

        public static DateTime SHOW_RECENT_COMPETITIONS_SINCE
        {
            get
            {
                return DateTime.Now.AddDays(-7);
            }
        }

        public static readonly int FETCH_BATCH_SIZE = 200;
        public static readonly int MINIMUM_SPACE_AHEAD = 3;
        public static readonly int MINIMUM_SPACE_BEHIND = 7;


        private static JArray? GetCompetitionGroupById(JArray? groups, string id)
        {
            JObject? group = groups?.Where(c => c["id"]?.Value<string>() == id)?.First() as JObject;
            if (group == null)
            {
                Log.Error(TAG, "Error fetching active competitions");
                return null;
            }
            return (group["competitions"] as JArray)!;
        }

        private static Task<LeaderboardSnapshot?> LoadCompetition(Dictionary<int, Leaderboard> competitions, JToken comp, Leaderboard? existing, int uid)
        {
            var name = (string)comp["name"]!;
            var startDate = new Timestamp(DateTime.Parse((string)comp["start_date"]!));
            var endDate = new Timestamp(DateTime.Parse((string)comp["end_date"]!));
            if (existing == null)
            {
                var id = (int)comp["id"]!;
                existing = new Leaderboard(id, null, null, null, name, LeaderboardType.Competition, startDate, endDate, new List<LeaderboardSnapshot>());
            }
            existing.LeaderboardName = name;
            existing.StartDate = startDate;
            existing.EndDate = endDate;


            competitions.Add(existing.LeaderboardId, existing);
            return existing.FetchSnapshot(uid);
        }

        public static async Task<Dictionary<int, LeaderboardSnapshot>?> FetchActiveCompetitions(int uid, Dictionary<int, Leaderboard> existing)
        {
            using WebClient wc = new WebClient();
            
            Log.Debug(TAG, "Fetching active competitions");
            var response = await wc.DownloadStringTaskAsync(COMPETITIONS_URL);
            JObject? json = null;
            try
            {
                json = JsonConvert.DeserializeObject<JObject>(response);
            }
            catch {
                return null;
            }

            var parsedResponse = json?["context"]?["c"]?["contests"];
            JArray activeCompetitions = GetCompetitionGroupById(parsedResponse as JArray, "featured") ?? new JArray();

            JArray finishedCompetitions = GetCompetitionGroupById(parsedResponse as JArray, "finished_competitions") ?? new JArray();
            var filteredFinishedCompetitions = finishedCompetitions.Where(x => existing.ContainsKey((int)x["id"]!) && DateTime.Parse((string)x["end_date"]!) > SHOW_RECENT_COMPETITIONS_SINCE).ToList();
            

            var competitions = new Dictionary<int, Leaderboard>();
            var tasks = new List<Task<LeaderboardSnapshot?>>();
            for (int i = 0; i < activeCompetitions.Count + filteredFinishedCompetitions.Count; i++)
            {
                var comp = i < activeCompetitions.Count ? activeCompetitions[i] : filteredFinishedCompetitions[i - activeCompetitions.Count];
                var existingComp = existing.GetValueOrDefault((int)comp["id"]!, null);
                tasks.Add(LoadCompetition(competitions, comp, existingComp, uid));
            }

            Log.Debug(TAG, "Fetching " + competitions.Count + " competitions");
            await Task.WhenAll(tasks);

            var res = tasks.Select(t => t.Result).Where(x => x != null).ToDictionary(x => x!.Parent.LeaderboardId);
            Log.Debug(TAG, "Fetched " + res.Count + " active competitions ");
            return res!;
        }

        public static async Task<LeaderboardSnapshot?> FetchLeaderboard(Leaderboard parent, int? uid, int leaderboardId, int? carId, int? classId, int? trackId)
        {
            var start = 0;
            int userIndex = -1;
            var entries = new List<LeaderboardEntry>();
            bool res = true;
            LeaderboardSnapshot snap = new LeaderboardSnapshot(new Timestamp(DateTime.Now), entries, parent);
            do
            {
                res = await FetchLeaderboard(entries, start, FETCH_BATCH_SIZE, leaderboardId, carId, classId, trackId);
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
            while ((userIndex == -1 || entries.Count - userIndex < MINIMUM_SPACE_BEHIND) && res);

            if (userIndex == -1)
            {
                Log.Warn(TAG, "User not found in leaderboard " + parent.ToString());
                return null;
            } else
            {
                Log.Info(TAG, "User found in leaderboard " + parent.ToString());
            }

            return snap;
        }

        public static async Task<bool> FetchLeaderboard(List<LeaderboardEntry> entries, int start, int count, int leaderboardId, int? carId, int? classId, int? trackId)
        {
            using WebClient wc = new WebClient();
            try
            {
                string url = string.Format(LEADERBOARD_LISTING_URL, leaderboardId);

                var queryParams = new Dictionary<string, string>()
                {
                    { "start", start.ToString() },
                    { "count", count.ToString() },
                };
                if (carId != null)
                {
                    queryParams.Add("car_class", carId.ToString());
                }
                else if (classId != null)
                {
                    queryParams.Add("car_class", classId.ToString());
                }
                if (trackId != null)
                {
                    queryParams.Add("track", trackId.ToString());
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
                if (data.Count == 0)
                {
                    return false;
                }
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
    }
}