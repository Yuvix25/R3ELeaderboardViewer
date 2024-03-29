﻿using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Fragment = AndroidX.Fragment.App.Fragment;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.Navigation;
using R3ELeaderboardViewer.Fragments;
using System.Threading.Tasks;
using AndroidX.Activity.Result;
using R3ELeaderboardViewer.Firebase;
using Android.Util;
using AndroidX.Lifecycle;

namespace R3ELeaderboardViewer
{
    public class ActivityResultCallback : Java.Lang.Object, IActivityResultCallback
    {
        readonly Action<ActivityResult> _callback;
        public ActivityResultCallback(Action<ActivityResult> callback) => _callback = callback;
        public ActivityResultCallback(TaskCompletionSource<ActivityResult> tcs) => _callback = tcs.SetResult;
        public void OnActivityResult(Java.Lang.Object p0) => _callback((ActivityResult)p0);
    }

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(MainActivity).Name;

        private MainFragment mainFragment;
        private LeaderboardViewerFragment leaderboardViewerFragment;

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

            mainFragment = new MainFragment();
            leaderboardViewerFragment = new LeaderboardViewerFragment();


            // Not the same as GotoFragment, using `Add` instead of `Replace`
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.fragment_container, mainFragment);
            transaction.Commit();


            FirebaseManager.Initialize(this);
        }

        public void GotoFragment(Fragment fragment)
        {
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.fragment_container, fragment);
            transaction.Commit();
        }

        public void GotoLeaderboardViewer(LeaderboardSnapshot snapshot)
        {
            GotoFragment(leaderboardViewerFragment);
            leaderboardViewerFragment.LoadSnapshot(snapshot);
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
                NavigateToFragment(mainFragment);
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
                FirebaseManager.GoogleSignInAsync();
            }
            else if (id == Resource.Id.nav_signout)
            {
                FirebaseManager.SignOut();
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

