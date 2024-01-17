using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Fragment = AndroidX.Fragment.App.Fragment;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Firebase;
using Google.Android.Material.Navigation;
using R3ELeaderboardViewer.Fragments;
using Android.Gms.Auth.Api.SignIn;
using AndroidX.Activity.Result.Contract;
using System.Threading.Tasks;
using Android.Widget;
using Android.Gms.Auth.Api;
using AndroidX.Activity.Result;

namespace R3ELeaderboardViewer
{
    public class ActivityResultCallback : Java.Lang.Object, IActivityResultCallback
    {
        readonly Action<ActivityResult> _callback;
        public ActivityResultCallback(Action<ActivityResult> callback) => _callback = callback;
        public ActivityResultCallback(TaskCompletionSource<ActivityResult> tcs) => _callback = tcs.SetResult;
        public void OnActivityResult(Java.Lang.Object p0) => _callback((ActivityResult)p0);
    }

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private MainFragment _mainFragment;

        private GoogleSignInOptions gso;
        private GoogleSignInClient googleSignInClient;
        public GoogleSignInAccount GoogleSignInAccount { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
            var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout)!;
            var toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view)!;
            navigationView.SetNavigationItemSelectedListener(this);


            // firebase
            FirebaseApp.InitializeApp(this);
            LoadGoogleSignInAccount(GoogleSignIn.GetLastSignedInAccount(this));

            // google sign in options
            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken("342595903850-ov63u6vea9ogo34qqsgd4t1eho0tbkdf.apps.googleusercontent.com")
                .RequestEmail()
                .Build();

            // google sign in client
            googleSignInClient = GoogleSignIn.GetClient(this, gso);
            GoogleSignInAsync = PrepareGoogleSignIn();


            _mainFragment = new MainFragment();
            
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.fragment_container, _mainFragment);
            transaction.Commit();
        }

        public void LoadGoogleSignInAccount(GoogleSignInAccount googleSignInAccount)
        {
            GoogleSignInAccount = googleSignInAccount;
            _mainFragment?.UpdateHelloMessage();
            _mainFragment?.SetRaceRoomUsernameInputVisibility(googleSignInAccount != null);
        }

        public Func<Task<GoogleSignInResult>> GoogleSignInAsync { get; private set; }
        private Func<Task<GoogleSignInResult>> PrepareGoogleSignIn()
        {
            TaskCompletionSource<GoogleSignInResult> taskCompletionSource = null;
            var activityResultLauncher = RegisterForActivityResult(
                new ActivityResultContracts.StartActivityForResult(),
                new ActivityResultCallback(
                    activityResult => taskCompletionSource?.SetResult(Auth.GoogleSignInApi.GetSignInResultFromIntent(activityResult.Data))
                )
            );

            return () => {
                taskCompletionSource = new TaskCompletionSource<GoogleSignInResult>();
                activityResultLauncher.Launch(googleSignInClient.SignInIntent);
                return taskCompletionSource.Task;
            };
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout)!;
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return true;
        }
        
        private void NavigateToFragment(Fragment fragment)
        {
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.fragment_container, fragment);
            transaction.Commit();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            
            if (id == Resource.Id.nav_home)
            {
                NavigateToFragment(_mainFragment);
            }
            else if (id == Resource.Id.nav_gallery)
            {

            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_settings)
            {

            }
            else if (id == Resource.Id.nav_signin)
            {
                // move to sign_in.xml:
                _ = GoogleSignInAsync().ContinueWith(task =>
                {
                    var result = task.Result;

                    Console.WriteLine(result.Status);
                    if (result.IsSuccess)
                    {
                        var account = result.SignInAccount;
                        LoadGoogleSignInAccount(account);
                        Console.WriteLine("Signed in as " + account.DisplayName);
                    }
                });
            }
            else if (id == Resource.Id.nav_signout)
            {
                googleSignInClient.SignOut();
                LoadGoogleSignInAccount(null);
            }

            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout)!;
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

