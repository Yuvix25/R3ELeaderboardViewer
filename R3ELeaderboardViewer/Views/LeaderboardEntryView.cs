using Android.Content;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using R3ELeaderboardViewer.Firebase;

namespace R3ELeaderboardViewer.Views
{
    public class LeaderboardEntryView : TableRow
    {
        private Drawable? CountryFlag;
        private Drawable? ProfileImage;
        private Drawable? LiveryImage;
        private int? Position;
        private int? Uid;
        private string Name;
        private string Laptime;
        private GameplayDifficulty Difficulty;
        private string? Team;
        private double? Points;
        private long? Timestamp;

        public LeaderboardEntryView(Context context) : base(context)
        {
            Initialize(context, null);
        }

        public LeaderboardEntryView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context, attrs);
        }

        private void Initialize(Context context, IAttributeSet attrs)
        {
            var padding1 = Utils.DpToPixel(1, context);
            var padding2 = Utils.DpToPixel(2, context);
            SetPadding(padding2, padding1, padding2, padding1);
            if (attrs != null)
            {
                var attributes = context.ObtainStyledAttributes(attrs, Resource.Styleable.LeaderboardEntryView);

                CountryFlag = attributes.GetDrawable(Resource.Styleable.LeaderboardEntryView_country_flag);
                ProfileImage = attributes.GetDrawable(Resource.Styleable.LeaderboardEntryView_profile_image);
                LiveryImage = attributes.GetDrawable(Resource.Styleable.LeaderboardEntryView_livery_image);
                Position = attributes.GetInt(Resource.Styleable.LeaderboardEntryView_position, -1);
                Uid = attributes.GetInt(Resource.Styleable.LeaderboardEntryView_uid, -1);
                Name = attributes.GetString(Resource.Styleable.LeaderboardEntryView_name);
                Laptime = attributes.GetString(Resource.Styleable.LeaderboardEntryView_laptime);
                Difficulty = (GameplayDifficulty)attributes.GetInt(Resource.Styleable.LeaderboardEntryView_difficulty, -1);
                Team = attributes.GetString(Resource.Styleable.LeaderboardEntryView_team);
                Points = attributes.GetFloat(Resource.Styleable.LeaderboardEntryView_points, -1);
                Timestamp = attributes.GetInt(Resource.Styleable.LeaderboardEntryView_timestamp, -1);

                Nullify();

                attributes.Recycle();

                LoadAttributes();
            }
        }

        private void Nullify()
        {
            Position = Position == -1 ? null : Position;
            Points = Points == -1 ? null : Points;
            Timestamp = Timestamp == -1 ? null : Timestamp;
        }

        public void LoadFromLeadeboardEntry(LeaderboardEntry entry)
        {
            Position = entry.Position;
            Uid = entry.Uid;
            Name = entry.DisplayName;
            Laptime = Utils.StringifyLaptime(entry.Laptime);
            Difficulty = entry.Difficulty;
            Team = entry.Team;
            Points = (float?)entry.Points;
            Timestamp = entry.Timestamp.ToDateTimeOffset().ToUnixTimeMilliseconds();

            Nullify();

            LoadAttributes();
        }
        public void LoadAttributes()
        {
            var strings = new string[]
            {
                Position.ToString(),
                Name,
                Laptime,
                Team,
            };
            for (var i = 0; i < strings.Length; i++)
            {
                LoadText(strings[i], i);
            }
        }

        private ImageView LoadDrawable(Drawable? drawable)
        {
            var view = new ImageView(Context);
            SetRightMargin(view);
            if (drawable != null)
            {
                view.SetImageDrawable(drawable);
                view.Visibility = ViewStates.Visible;
            }
            else
            {
                view.Visibility = ViewStates.Gone;
            }
            AddView(view);

            return view;
        }

        private TextView LoadText(string? text, int index)
        {
            TextView view;
            if (index < ChildCount)
            {
                view = (TextView)GetChildAt(index);
            }
            else
            {
                view = new TextView(Context);
            }

            view.SetTextAppearance(Resource.Style.LeaderboardEntryTextStyle);
            view.SetMaxLines(1);
            view.SetMaxEms(11);
            view.Ellipsize = TextUtils.TruncateAt.End;
            SetRightMargin(view);
            if (text != null)
            {
                view.Text = text;
                view.Visibility = ViewStates.Visible;
            }
            else
            {
                view.Visibility = ViewStates.Gone;
            }

            if (index >= ChildCount)
            {
                AddView(view);
            }
            return view;
        }

        private void SetRightMargin(View view)
        {
            view.LayoutParameters = new TableRow.LayoutParams()
            {
                RightMargin = Utils.DpToPixel(8, Context),
            };
        }
    }
}