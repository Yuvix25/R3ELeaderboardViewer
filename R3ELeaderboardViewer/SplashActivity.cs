using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.AppCompat.App;
using R3ELeaderboardViewer.Firebase;
using System;
using System.Threading.Tasks;

namespace R3ELeaderboardViewer
{
    [Activity(Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Log.Debug(TAG, "SplashActivity.OnCreate");

            Startup();
        }


        void Startup()
        {
            Log.Debug(TAG, "Starting up...");
            Action remove = null;

            remove = FirebaseManager.OnFirebaseUser((userData) =>
            {
                Log.Debug(TAG, "Startup work is finished - starting MainActivity");
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                remove();
            }, false);

            FirebaseManager.Initialize((Context)this);
        }
    }
}