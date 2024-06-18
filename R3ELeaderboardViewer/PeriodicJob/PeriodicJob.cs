using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using R3ELeaderboardViewer.Firebase;
using System.Threading.Tasks;

namespace R3ELeaderboardViewer.PeriodicJob
{
    [Service]
    public class PeriodicJob : Service
    {
        public static readonly string TAG = "R3ELeaderboardViewer:" + typeof(PeriodicJob).Name;
        public static readonly string MAIN_NOTIFICATION_TITLE = "R3E Leaderboard Viewer";

        private void StartForegroundService()
        {
            Log.Info(TAG, "PeriodicJob.StartForegroundService");
            var intent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.Immutable);

            Notifications.Initialize(this);
            Notifications.CreateNotificationForeground(MAIN_NOTIFICATION_TITLE, "Querying leaderboards...", pendingIntent);
        }

        private async Task DoWork()
        {
            // firebase initialization ultimately leads to fetching all new competitions and sending notifications
            await FirebaseManager.Initialize(this, onlySignedIn: true);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Info(TAG, "PeriodicJob.OnStartCommand");
            StartForegroundService();
            _ = DoWork();

            return StartCommandResult.RedeliverIntent;
        }
    }
}