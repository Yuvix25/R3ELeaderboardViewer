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


        async void Startup()
        {
            Log.Debug(TAG, "Starting up...");

            var firebaseTask = FirebaseManager.Initialize((Context)this);
            Utils.CountryFlags.LoadCountryFlagsAsync(this); // eager load country flags to prevent slight lag when loading leaderboard

            await Task.WhenAll(firebaseTask);

            Log.Debug(TAG, "Startup work is finished - starting MainActivity");
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}