using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using R3ELeaderboardViewer.Adapters;
using R3ELeaderboardViewer.Firebase;
using R3ELeaderboardViewer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace R3ELeaderboardViewer.Fragments
{
    public class MainFragment : AndroidX.Fragment.App.Fragment
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(MainFragment).Name;

        private LinearLayout RaceRoomUsernameBlock;
        private EditText RaceRoomUsernameInput;
        private Button RaceRoomUsernameSaveButton;
        private ImageView RaceRoomUsernameValidImage;
        private ExpandableListView LeaderboardElv;
        private TextView HelloMessage;
        private int ValidImage = Resource.Drawable.ic_check;
        private int InvalidImage = Resource.Drawable.ic_close;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.home_screen, container, false);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            RaceRoomUsernameBlock = view.FindViewById<LinearLayout>(Resource.Id.raceroom_data);
            RaceRoomUsernameInput = view.FindViewById<EditText>(Resource.Id.raceroom_username_input);
            RaceRoomUsernameSaveButton = view.FindViewById<Button>(Resource.Id.raceroom_username_save);
            RaceRoomUsernameValidImage = view.FindViewById<ImageView>(Resource.Id.raceroom_username_valid);
            LeaderboardElv = view.FindViewById<ExpandableListView>(Resource.Id.home_leaderboard_elv);
            HelloMessage = view.FindViewById<TextView>(Resource.Id.hello_message);

            RaceRoomUsernameInput.TextChanged += (sender, args) =>
            {
                if (FirebaseManager.UserData == null)
                {
                    return;
                }
                if (RaceRoomUsernameInput.Text != FirebaseManager.UserData.RaceRoomUsername)
                {
                    SetRaceRoomUsernameValid(null);
                }
            };
            RaceRoomUsernameSaveButton.Click += (sender, args) => SaveRaceRoomUsername();

            FirebaseManager.OnUser(UserUpdated);
            FirebaseManager.OnFirebaseUser(FirebaseUserUpdated);
        }

        private void OnLeaderboardClick(LeaderboardSnapshot leaderboardSnapshot)
        {
            Log.Debug(TAG, "Loading leaderboard: " + leaderboardSnapshot.Parent.LeaderboardName);
            ((MainActivity)Activity).GotoLeaderboardViewer(leaderboardSnapshot);
        }

        public void UserUpdated(GoogleSignInAccount googleSignInAccount)
        {
            if (Activity == null)
            {
                Log.Warn(TAG, "Activity is null");
                return;
            }
            Activity.RunOnUiThread(() =>
            {
                UpdateHelloMessage(googleSignInAccount);
                SetRaceRoomUsernameBlockVisibility(googleSignInAccount != null);
            });
        }

        public void FirebaseUserUpdated(UserData userData)
        {
            Activity.RunOnUiThread(() =>
            {
                try
                {
                    RaceRoomUsernameInput.Text = userData?.RaceRoomUsername ?? "";
                    SetRaceRoomUsernameValid();

                    var elvStructure = new Dictionary<string, List<LeaderboardSnapshot>>();
                    var recentCompetitionSnapshots = GetLatestSnapshots(userData?.RecentCompetitions);
                    var savedLeaderboardSnapshots = GetLatestSnapshots(userData?.SavedLeaderboards);
                    elvStructure.Add("Recent Competitions", recentCompetitionSnapshots);
                    elvStructure.Add("Saved Leaderboards", savedLeaderboardSnapshots);
                    var leaderboardAdapter = new LeaderboardExpandableListViewAdapter(Context, elvStructure.Keys.ToList(), elvStructure);
                    leaderboardAdapter.OnLeaderboardClick += OnLeaderboardClick;
                    LeaderboardElv.SetAdapter(leaderboardAdapter);
                }
                catch (Exception e)
                {
                    Log.Debug(TAG, "Error updating Firebase user: " + e);
                }
            });
        }

        private static List<LeaderboardSnapshot> GetLatestSnapshots(List<Leaderboard> competitions)
        {
            var snapshots = new List<LeaderboardSnapshot>();
            foreach (var competition in competitions)
            {
                if (competition.Snapshots.Count > 0)
                {
                    snapshots.Add(competition.Snapshots[^1]);
                }
            }
            return snapshots;
        }

        public void UpdateHelloMessage(GoogleSignInAccount account)
        {
            var displayName = account?.DisplayName;
            if (displayName != null)
            {
                HelloMessage.Text = $"Hello {displayName}!";
            }
            else
            {
                HelloMessage.Text = "Hello! Please sign in from the sidebar.";
            }
        }
        public void UpdateHelloMessage()
        {
            UpdateHelloMessage(FirebaseManager.GoogleSignInAccount);
        }

        public void SetRaceRoomUsernameBlockVisibility(bool visible)
        {
            RaceRoomUsernameBlock.Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
        }

        public async void SaveRaceRoomUsername()
        {
            var username = RaceRoomUsernameInput.Text;
            username = username.Length == 0 ? null : username;
            
            var valid = await FirebaseManager.UserData.SaveRaceRoomUsername(username);
            SetRaceRoomUsernameValid(valid);
            if (!valid)
            {
                RaceRoomUsernameInput.ClearFocus();
                await Task.Delay(700);
                RaceRoomUsernameInput.Text = FirebaseManager.UserData?.RaceRoomUsername ?? "";
                SetRaceRoomUsernameValid();
            }
        }

        public void SetRaceRoomUsernameValid(bool? valid)
        {
            if (valid == null)
            {
                RaceRoomUsernameValidImage.SetColorFilter(Android.Graphics.Color.Transparent);
                return;
            }
            RaceRoomUsernameValidImage.SetImageResource((bool)valid ? ValidImage : InvalidImage);
            RaceRoomUsernameValidImage.SetColorFilter((bool)valid ? Android.Graphics.Color.Green : Android.Graphics.Color.Red);
        }

        public void SetRaceRoomUsernameValid()
        {
            SetRaceRoomUsernameValid(RaceRoomUsernameInput.Text != "");
        }
    }
}