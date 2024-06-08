using Android.OS;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using R3ELeaderboardViewer.Firebase;
using R3ELeaderboardViewer.Fragments;
using R3ELeaderboardViewer.Views;

namespace R3ELeaderboardViewer
{
    public class LeaderboardViewerFragment : AndroidX.Fragment.App.Fragment
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(LeaderboardViewerFragment).Name;

        private SwipeRefreshLayout SwipeRefreshLayout = null;
        private LeaderboardView LeaderboardView = null;
        private ScrollView ScrollView = null;

        private delegate void LeaderboardViewLoadedDelegate(LeaderboardView view);
        private event LeaderboardViewLoadedDelegate OnLeaderboardViewLoaded = delegate { };
        private void OnceLeaderboardViewLoaded(LeaderboardViewLoadedDelegate callback)
        {
            if (LeaderboardView != null)
            {
                callback(LeaderboardView);
            }
            else
            {
                OnLeaderboardViewLoaded += callback;
            }
        }

        private LeaderboardSnapshot leaderboardSnapshot;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.leaderboard_viewer, container, false);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SwipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.leaderboard_refresher);
            LeaderboardView = view.FindViewById<LeaderboardView>(Resource.Id.leaderboard_view);
            ScrollView = view.FindViewById<ScrollView>(Resource.Id.leaderboard_scroll);
            OnLeaderboardViewLoaded?.Invoke(LeaderboardView);
            OnLeaderboardViewLoaded = delegate { };

            Log.Debug(TAG, "OnViewCreated " + LeaderboardView.EntryCount);


            SwipeRefreshLayout.Refresh += OnRefresh;

            //FirebaseManager.OnUser(UserUpdated);
            //FirebaseManager.OnFirebaseUser(FirebaseUserUpdated);
        }

        private void OnRefresh(object sender, System.EventArgs e)
        {
            OnceLeaderboardViewLoaded(async (view) => {
                if (leaderboardSnapshot != null)
                {
                    int? uid = null;
                    if (leaderboardSnapshot.UserIndex != null)
                    {
                        uid = leaderboardSnapshot.Entries[leaderboardSnapshot.UserIndex.Value].Uid;
                    }
                    LoadSnapshot(await leaderboardSnapshot.Parent.FetchSnapshot(uid, true));
                }
                SwipeRefreshLayout.Refreshing = false;
            });
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            SwipeRefreshLayout.Refresh -= OnRefresh;

            SwipeRefreshLayout = null;
            LeaderboardView = null;
            ScrollView = null;
        }

        public void LoadSnapshot(LeaderboardSnapshot snapshot)
        {
            leaderboardSnapshot = snapshot;
            OnceLeaderboardViewLoaded((view) => {
                view.LoadFullLeaderboard(snapshot);
                ScrollView.Post(() =>
                {
                    ScrollView.ScrollTo(0, view.Me.Top);
                });
            });
        }
    }
}