using Android.Util;
using Firebase.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.CloudFirestore;
using R3ELeaderboardViewer.RaceRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

#nullable enable

namespace R3ELeaderboardViewer.Firebase
{
    public class UserData : Entity
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(UserData).Name;

        static UserData()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public FirebaseUser? FirebaseUser { get; internal set; } = null;

        [FirebaseField]
        public string? RaceRoomUserName { get; private set; }

        [FirebaseField]
        public int? RaceRoomUserId { get; private set; }

        [FirebaseCollection]
        public Dictionary<string, Leaderboard> RecentCompetitions { get; private set; } = new Dictionary<string, Leaderboard>();


        public UserData(FirebaseUser firebaseUser)
        {
            FirebaseUser = firebaseUser;
            Reference = CrossCloudFirestore.Current.Instance.Collection("Users").Document(FirebaseUser.Uid);
        }

        protected override Task OnFirebasePopulate()
        {
            return LoadRaceRoomUser();
        }

        public async Task<bool> SaveRaceRoomUserName(string raceRoomUserName, Action<UserData> callback)
        {
            var id = await ValidateRaceRoomUserName(raceRoomUserName);
            if (id == null && raceRoomUserName != null)
            {
                return false;
            }
            if (RaceRoomUserName != raceRoomUserName) {
                var oldId = RaceRoomUserId;
                RaceRoomUserName = raceRoomUserName;
                RaceRoomUserId = id;

                var oldRecentCompetitions = RecentCompetitions;
                if (oldId != null)
                {
                    RecentCompetitions.Clear();
                    callback(this);
                }
                var transaction = CrossCloudFirestore.Current.Instance.RunTransactionAsync(async transaction =>
                {
                    await SaveFields();

                    if (oldId != null)
                    {
                        var tasks = oldRecentCompetitions.Values.Select(comp => comp.Delete()).ToArray();
                        await Task.WhenAll(tasks);
                    }
                });
                _ = transaction.ContinueWith(task =>
                {
                    task.Result.ContinueWith(async task =>
                    {
                        await LoadRaceRoomUser();
                        callback(this);
                    });
                });

                return raceRoomUserName != null;
            }
            return false;
        }

        public async Task<int?> ValidateRaceRoomUserName(string raceRoomUsername)
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
                var response = await wc.DownloadStringTaskAsync(string.Format(RaceRoomApiManager.USER_INFO_URL, raceRoomUsername));
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

        private async Task LoadRaceRoomUser()
        {
            if (RaceRoomUserId == null)
            {
                return;
            }

            await LoadRecentCompetitions();
        }

        public struct RecentCompetitionsResult
        {
            public IEnumerable<Leaderboard> NewCompetitions;
            public IEnumerable<Leaderboard> ExistingCompetitions;
            public IEnumerable<Leaderboard> ExpiredCompetitions;
        }

        public RecentCompetitionsResult? LatestRecentCompetitionsResult { get; private set; } = null;

        public async Task LoadRecentCompetitions()
        {
            try
            {
                var newSnaps = await RaceRoomApiManager.FetchActiveCompetitions((int)RaceRoomUserId, RecentCompetitions.ToDictionary(x => x.Value.LeaderboardId, x => x.Value)) ?? new Dictionary<int, LeaderboardSnapshot>();
                var expired = RecentCompetitions.Where(pair => !newSnaps.ContainsKey(pair.Value.LeaderboardId));
                var existing = RecentCompetitions.Where(pair => newSnaps.ContainsKey(pair.Value.LeaderboardId));
                var newComps = newSnaps.Values.Select(snap => snap.Parent).Where(comp => !RecentCompetitions.ContainsKey(comp.FirebaseId)).ToArray();
                Log.Debug(TAG, "Found " + expired.Count() + " competitions that are inactive for over a week");
                await (await CrossCloudFirestore.Current.Instance.RunTransactionAsync(async transaction =>
                {
                    foreach (var snap in newSnaps)
                    {
                        var comp = snap.Value.Parent;
                        if (comp.Reference == null)
                        {
                            comp.SetAsCompetition(this);
                        }
                        await comp.AddSnapshot(snap.Value);
                    }
                    Task.WaitAll(expired.Select(async comp =>
                    {
                        await comp.Value.Delete();
                    }).ToArray());
                    foreach (var comp in newComps)
                    {
                        await comp.SaveFields();
                    }
                    RecentCompetitions = newSnaps.ToDictionary(pair => pair.Value.Parent.FirebaseId, pair => pair.Value.Parent);
                }));

                LatestRecentCompetitionsResult = new RecentCompetitionsResult
                {
                    NewCompetitions = newComps,
                    ExistingCompetitions = existing.Select(pair => pair.Value),
                    ExpiredCompetitions = expired.Select(pair => pair.Value),
                };
                Notifications.CreateNotificationsForCompetitions(this);
            }
            catch (WebException e)
            {
                Log.Error(TAG, "Error fetching recent competitions " + e);
            }
        }
    }

    public class Leaderboard : Entity
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(Leaderboard).Name;

        [FirebaseField]
        public int LeaderboardId;

        [FirebaseField]
        public int? CarId;

        [FirebaseField]
        public int? ClassId;

        [FirebaseField]
        public int? TrackId;

        [FirebaseField]
        public string LeaderboardName;

        [FirebaseField]
        public LeaderboardType Type;

        [FirebaseField]
        public Timestamp? StartDate;

        [FirebaseField]
        public Timestamp? EndDate;

        [FirebaseCollection]
        public List<LeaderboardSnapshot> Snapshots { get; internal set; } = new List<LeaderboardSnapshot>();


        public string FirebaseId { get
            {
                return $"{Type}-{LeaderboardId}" + (Type == LeaderboardType.Leaderboard ? $"-{CarId ?? ClassId}-{TrackId}" : "");
            }
        }

        public async Task AddSnapshot(LeaderboardSnapshot snapshot)
        {
            if (Snapshots.Count > 1)
            {
                // remove all except the last snapshot
                foreach (var snap in Snapshots.GetRange(0, Snapshots.Count - 1))
                {
                    await snap.Delete();
                }
                Snapshots = Snapshots.GetRange(Snapshots.Count - 1, 1);
            }

            Snapshots.Add(snapshot);
            await snapshot.Save();
        }


        public Leaderboard(int leaderboardId, int? carId, int? classId, int? trackId, string leaderboardName, LeaderboardType leaderboardType, Timestamp startDate, Timestamp endDate, List<LeaderboardSnapshot> snapshots)
        {
            LeaderboardId = leaderboardId;
            CarId = carId;
            ClassId = classId;
            TrackId = trackId;
            LeaderboardName = leaderboardName;
            Type = leaderboardType;
            StartDate = startDate;
            EndDate = endDate;
            Snapshots = snapshots;
        }

        // for reflection
        public Leaderboard() { }
        
        public void SetAsCompetition(UserData user)
        {
            Reference = user.Reference.Collection("RecentCompetitions").Document(FirebaseId);
        }

        public void SetAsLeaderboard(UserData user)
        {
            Reference = user.Reference.Collection("SavedLeaderboards").Document(FirebaseId);
        }

        public IDocumentReference ReferenceFor(LeaderboardSnapshot snapshot)
        {
            return Reference.Collection("Snapshots").Document(snapshot.Timestamp.Seconds.ToString());
        }

        public async Task<LeaderboardSnapshot?> FetchSnapshot(int? uid, bool save = false)
        {
            var snap = await RaceRoomApiManager.FetchLeaderboard(this, uid, LeaderboardId, CarId, ClassId, TrackId);
            Log.Debug(TAG, "Fetched snapshot " + snap);
            if (snap == null)
            {
                return null;
            }
            
            if (save)
            {
                await AddSnapshot(snap);
            }

            return snap;
        }

        public override string ToString()
        {
            return $"Leaderboard(LeaderboardId={LeaderboardId}, CarId={CarId}, ClassId={ClassId}, TrackId={TrackId}, LeaderboardName={LeaderboardName}, Type={Type}, StartDate={(StartDate.HasValue ? StartDate.Value.ToDateTime().ToString() : "NULL")}, EndDate={(EndDate.HasValue ? EndDate.Value.ToDateTime().ToString() : "NULL")}, Snapshots-{Snapshots.Count})";
        }
    }

    public class LeaderboardSnapshot : Entity
    {
        [FirebaseField]
        public readonly Timestamp Timestamp;

        [FirebaseField]
        public int? UserIndex { get; internal set; }

        [FirebaseField]
        public readonly List<LeaderboardEntry> Entries;

        [FirebaseParent]
        public readonly Leaderboard Parent;

        public override IDocumentReference? Reference
        {
            get
            {
                return Parent.ReferenceFor(this);
            }
        }

        // for reflection
        public LeaderboardSnapshot() { }

        public LeaderboardSnapshot(Timestamp timestamp, List<LeaderboardEntry> entries, Leaderboard parent)
        {
            Timestamp = timestamp;
            Entries = entries;
            Parent = parent;
        }

        public override string ToString()
        {
            return $"LeaderboardSnapshot(Timestamp={Timestamp.ToDateTime()}, UserIndex={UserIndex}, Entries={Entries.Count})";
        }
    }

    public class LeaderboardEntry : JsonEntity
    {
        [FirebaseField]
        public readonly string CountryCode;

        [FirebaseField]
        public readonly int Position;

        [FirebaseField]
        public readonly string? ProfilePictureUrl;

        [FirebaseField]
        public readonly int? Uid;

        [FirebaseField]
        public readonly string DisplayName;

        [FirebaseField]
        public readonly double Laptime;

        [FirebaseField]
        public readonly double? Gap;

        [FirebaseField]
        public readonly string? LiveryImageUrl;

        [FirebaseField]
        public readonly string? TrackImageUrl;

        [FirebaseField]
        public readonly GameplayDifficulty Difficulty;

        [FirebaseField]
        public readonly string Team;

        [FirebaseField]
        public readonly double? Points;

        [FirebaseField]
        public readonly Timestamp Timestamp;

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

        // for reflection
        public LeaderboardEntry() { }


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
        Unknown = -1,
        Leaderboard = 0,
        Competition = 1,
    }
}