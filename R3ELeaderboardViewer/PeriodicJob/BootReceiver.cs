using Android.App;
using Android.Content;

namespace R3ELeaderboardViewer.PeriodicJob
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals(Intent.ActionBootCompleted))
            {
                // Reschedule the task
                Intent serviceIntent = new Intent(context, typeof(MainActivity));
                serviceIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(serviceIntent);
            }
        }
    }
}