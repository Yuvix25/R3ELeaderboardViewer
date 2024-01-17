using Android.OS;
using Android.Views;
using Android.Widget;

namespace R3ELeaderboardViewer.Fragments
{
    public class MainFragment : AndroidX.Fragment.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.home_screen, container, false);

            UpdateHelloMessage();

            return view;
        }

        public void UpdateHelloMessage()
        {
            var displayName = ((MainActivity)Activity).GoogleSignInAccount?.DisplayName;
            var textView = View?.FindViewById<TextView>(Resource.Id.hello_message);
            if (textView == null)
            {
                return;
            }
            if (displayName != null)
            {
                textView.Text = $"Hello {displayName}!";
            }
            else
            {
                textView.Text = "Hello! Please sign in from the sidebar.";
            }
        }

        public void SetRaceRoomUsernameInputVisibility(bool visible)
        {
            var input = View?.FindViewById<EditText>(Resource.Id.raceroom_username_input);
            if (input == null)
            {
                return;
            }
            input.Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}