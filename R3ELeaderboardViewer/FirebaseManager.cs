
using System;
using Firebase;
using Firebase.Auth;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using System.Threading.Tasks;
using AndroidX.Activity.Result.Contract;
using AndroidX.AppCompat.App;
using Android.Util;
using Android.Content;

namespace R3ELeaderboardViewer.Firebase
{
    public static class FirebaseManager
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(FirebaseManager).Name;

        private static AppCompatActivity activity = null;

        private static GoogleSignInOptions gso = null;
        private static GoogleSignInClient googleSignInClient = null;
        public static Func<Task<GoogleSignInResult>> GoogleSignInAsync { get; private set; } = null;

        public delegate void OnUserDelegate(GoogleSignInAccount googleSignInAccount);
        private static OnUserDelegate _OnUser = delegate { };

        public static Action OnUser(OnUserDelegate onUserDelegate)
        {
            onUserDelegate.Invoke(GoogleSignInAccount);
            _OnUser += onUserDelegate;
            return () => _OnUser -= onUserDelegate;
        }

        public delegate void OnFirebaseUserDelegate(UserData userData);
        public static OnFirebaseUserDelegate _OnFirebaseUser = (UserData userData) =>
        {

        };

        public static Action OnFirebaseUser(OnFirebaseUserDelegate onFirebaseUserDelegate, bool invokeNow = true)
        {
            if (invokeNow)
            {
                onFirebaseUserDelegate.Invoke(UserData);
            }
            _OnFirebaseUser += onFirebaseUserDelegate;
            return () => _OnFirebaseUser -= onFirebaseUserDelegate;
        }

        public static Task<UserData> WaitForFirebaseUser(bool invokeNow = true)
        {
            var tcs = new TaskCompletionSource<UserData>();
            Action remove = null;
            remove = OnFirebaseUser((userData) =>
            {
                tcs.TrySetResult(userData);
                remove?.Invoke();
            }, invokeNow);
            return tcs.Task;
        }

        public static UserData UserData { get; private set; } = null;
        public static GoogleSignInAccount GoogleSignInAccount { get; private set; } = null;

        public async static Task Initialize(Context context)
        {
            FirebaseApp.InitializeApp(context);

            // google sign in options
            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(context.GetString(Resource.String.default_web_client_id))
                .RequestEmail()
                .Build();


            var lastSignedInAccount = GoogleSignIn.GetLastSignedInAccount(context);

            Log.Debug(TAG, "Google Current User: " + lastSignedInAccount?.DisplayName);
            Log.Debug(TAG, "Firebase Current User: " + FirebaseAuth.Instance.CurrentUser?.DisplayName);
            await LoadGoogleSignInAccount(lastSignedInAccount);
        }
        public static void Initialize(AppCompatActivity activity)
        {
            FirebaseManager.activity = activity;

            try
            {
                // google sign in client
                googleSignInClient = GoogleSignIn.GetClient(activity, gso);
                PrepareGoogleSignIn();
            }
            catch (Exception e)
            {
                Log.Error(TAG, "Error initializing Google Sign In: " + e);
            }
        }

        public static Func<Task<GoogleSignInResult>> PrepareGoogleSignIn()
        {
            if (GoogleSignInAsync != null)
            {
                return GoogleSignInAsync;
            }
            googleSignInClient = GoogleSignIn.GetClient(activity, gso);
            TaskCompletionSource<GoogleSignInResult> taskCompletionSource = null;
            var activityResultLauncher = activity.RegisterForActivityResult(
                new ActivityResultContracts.StartActivityForResult(),
                new ActivityResultCallback(
                    activityResult => taskCompletionSource?.SetResult(Auth.GoogleSignInApi.GetSignInResultFromIntent(activityResult.Data))
                )
            );

            GoogleSignInAsync = () => {
                taskCompletionSource = new TaskCompletionSource<GoogleSignInResult>();
                activityResultLauncher.Launch(googleSignInClient.SignInIntent);
                return taskCompletionSource.Task.ContinueWith((res) =>
                {
                    Log.Debug(TAG, "Google Sign In Sucessful: " + (res.Result.SignInAccount?.IdToken != null));
                    LoadGoogleSignInAccount(res.Result.SignInAccount);
                    return res.Result;
                });
            };
            return GoogleSignInAsync;
        }

        public static async Task LoadGoogleSignInAccount(GoogleSignInAccount googleSignInAccount)
        {
            GoogleSignInAccount = googleSignInAccount;
            if (googleSignInAccount?.IdToken == null)
            {
                SignOut();
            }
            else if (FirebaseAuth.Instance.CurrentUser == null)
            {
                try
                {
                    var res = await FirebaseAuth.Instance.SignInWithCredentialAsync(GoogleAuthProvider.GetCredential(googleSignInAccount?.IdToken, null));
                    await OnFirebaseUserLoaded(res.AdditionalUserInfo.IsNewUser);
                }
                catch (Exception e)
                {
                    Log.Error(TAG, "Error signing in Firebase user: " + e);
                }
            }
            else
            {
                await OnFirebaseUserLoaded(false);
            }

            if (googleSignInAccount != null)
            {
                _OnUser.Invoke(googleSignInAccount);
            }
        }

        public static void SignOut()
        {
            Log.Debug(TAG, "Signing out...");
            googleSignInClient?.SignOut();
            FirebaseAuth.Instance.SignOut();

            GoogleSignInAccount = null;
            UserData = null;
            _OnUser.Invoke(null);
            OnFirebaseUserRemove();
        }

        private static async Task OnFirebaseUserLoaded(bool isNewUser)
        {
            Log.Debug(TAG, "OnFirebaseUserLoaded");
            if (UserData == null)
            {
                UserData = new UserData(FirebaseAuth.Instance.CurrentUser);
            }
            else
            {
                UserData.FirebaseUser = FirebaseAuth.Instance.CurrentUser;
            }

            if (isNewUser)
            {
                await UserData.Save();
            }
            else
            {
                await UserData.Load();
            }

            _OnFirebaseUser.Invoke(UserData);
        }

        private static void OnFirebaseUserRemove()
        {
            Log.Debug(TAG, "OnFirebaseUserRemove");
            UserData = null;
            _OnFirebaseUser.Invoke(null);
        }
    }
}