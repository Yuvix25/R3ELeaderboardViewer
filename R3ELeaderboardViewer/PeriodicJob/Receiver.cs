using Android.Content;

namespace R3ELeaderboardViewer.PeriodicJob
{
    [BroadcastReceiver]
    public class Receiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Intent serviceIntent = new Intent(context, typeof(PeriodicJob));
            serviceIntent.AddFlags(ActivityFlags.NewTask);
            context.StartForegroundService(serviceIntent);
        }
    }
}