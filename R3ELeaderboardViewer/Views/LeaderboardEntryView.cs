using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content.Res;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Net;
using Plugin.CloudFirestore;
using R3ELeaderboardViewer.Firebase;
using System;
using System.Linq;
using Square.Picasso;

namespace R3ELeaderboardViewer.Views
{
    public class LeaderboardEntryView : TableRow
    {
        private Drawable? CountryFlag;
        private Drawable? ProfilePicture;
        private Drawable? LiveryPicture;
        private URL? ProfilePictureUrl;
        private URL? LiveryImageUrl;
        public int? Position { get; private set; }
        private int? Uid;
        private string Name;
        private string Laptime;
        private GameplayDifficulty Difficulty;
        private string? Team;
        private double? Points;
        private Timestamp? Timestamp;

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
                ProfilePicture = attributes.GetDrawable(Resource.Styleable.LeaderboardEntryView_profile_picture);
                LiveryPicture = attributes.GetDrawable(Resource.Styleable.LeaderboardEntryView_livery_image);
                Position = attributes.GetInt(Resource.Styleable.LeaderboardEntryView_position, -1);
                Uid = attributes.GetInt(Resource.Styleable.LeaderboardEntryView_uid, -1);
                Name = attributes.GetString(Resource.Styleable.LeaderboardEntryView_name);
                Laptime = attributes.GetString(Resource.Styleable.LeaderboardEntryView_laptime);
                Difficulty = (GameplayDifficulty)attributes.GetInt(Resource.Styleable.LeaderboardEntryView_difficulty, -1);
                Team = attributes.GetString(Resource.Styleable.LeaderboardEntryView_team);
                Points = attributes.GetFloat(Resource.Styleable.LeaderboardEntryView_points, -1);
                var timestampMilliseconds = attributes.GetInt(Resource.Styleable.LeaderboardEntryView_timestamp, -1);
                if (timestampMilliseconds == -1)
                {
                    Timestamp = null;
                }
                else
                {
                    Timestamp = new Timestamp(DateTimeOffset.FromUnixTimeMilliseconds(timestampMilliseconds));
                }

                Nullify();

                attributes.Recycle();

                LoadAttributes();
            }
        }

        private void Nullify()
        {
            Position = Position == -1 ? null : Position;
            Points = Points == -1 ? null : Points;
        }

        public string[] LoadFromLeadeboardEntry(LeaderboardEntry entry)
        {
            ProfilePictureUrl = new URL(entry.ProfilePictureUrl);
            LiveryImageUrl = new URL(entry.LiveryImageUrl);
            CountryFlag = Utils.CountryFlags.GetCountryFlagDrawable(Context, entry.CountryCode);
            Position = entry.Position;
            Uid = entry.Uid;
            Name = entry.DisplayName;
            Laptime = Utils.StringifyLaptime(entry.Laptime);
            Difficulty = entry.Difficulty;
            Team = entry.Team;
            Points = (float?)entry.Points;
            Timestamp = entry.Timestamp;

            Nullify();

            return LoadAttributes(new string[]
            {
                "CountryFlag",
                "Position",
                "Name",
                "Laptime",
                "Team",
            });
        }

        private static readonly object NullAttribute = new object();
        private object GetAttribute(string attr)
        {
            return attr switch
            {
                "CountryFlag" => CountryFlag ?? (object)CountryFlag,
                "ProfileImage" => ProfilePicture ?? (object)ProfilePictureUrl,
                "LiveryImage" => LiveryPicture ?? (object)LiveryImageUrl,
                "Position" => Position,
                "Uid" => Uid,
                "Name" => Name,
                "Laptime" => Laptime,
                "Difficulty" => Difficulty,
                "Team" => Team,
                "Points" => Points,
                "Timestamp" => Timestamp,
                _ => NullAttribute,
            };
        }

        public string[] LoadAttributes(string[] attrs = null)
        {
            attrs ??= new string[] { "Position", "Name", "Laptime", "Team" };
            if (attrs.Length < ChildCount)
            {
                RemoveViews(attrs.Length, ChildCount - attrs.Length);
            }

            var values = attrs.Select(GetAttribute).Where(x => x != NullAttribute).ToArray();
            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];
                if (value is Drawable drawable)
                {
                    LoadImage(drawable, i);
                }
                else if (value is URL url)
                {
                    LoadImage(url, i);
                }
                else if (value is string text)
                {
                    LoadText(text, i);
                }
                else if (value is int position)
                {
                    LoadText(position.ToString(), i);
                }
                else if (value is double points)
                {
                    LoadText(points.ToString("0.00"), i);
                }
                else if (value is Timestamp timestamp)
                {
                    LoadText(timestamp.ToDateTime().ToString(), i);
                }
                else if (value is GameplayDifficulty difficulty)
                {
                    LoadText(difficulty.ToString(), i);
                }
            }

            return attrs;
        }

        private T GetViewInPosition<T>(int position) where T : View
        {
            if (position < ChildCount)
            {
                var view = GetChildAt(position);
                if (view is T tView)
                {
                    return tView;
                }
                else
                {
                    RemoveViews(position, ChildCount - position);
                }
            }
            return null;
        }
        private ImageView LoadImage(Drawable image, int i)
        {
            return LoadImage((view) =>
            {
                if (image != null)
                {
                    view.SetImageDrawable(image);
                }
            }, i);
        }

        private ImageView LoadImage(URL url, int i)
        {
            return LoadImage((view) =>
            {
                Picasso.Get().Load(url.ToString()).Into(view);
            }, i);
        }

        private ImageView LoadImage(Action<ImageView> applyImage, int i)
        {
            ImageView view = GetViewInPosition<ImageView>(i) ?? new ImageView(Context);

            SetRightMargin(view);
            applyImage.Invoke(view);
            view.LayoutParameters.Width = Utils.DpToPixel(15, Context);
            view.LayoutParameters.Height = Utils.DpToPixel(18, Context);

            if (i >= ChildCount)
            {
                AddView(view);
            }
            else
            {
                view.RequestLayout();
            }

            return view;
        }

        private TextView LoadText(string? text, int index)
        {
            TextView view = GetViewInPosition<TextView>(index) ?? new TextView(Context);

            view.SetTextAppearance(Resource.Style.LeaderboardEntryTextStyle);
            view.SetMaxLines(1);
            view.SetMaxEms(11);
            view.Ellipsize = TextUtils.TruncateAt.End;
            SetRightMargin(view);
            if (text != null)
            {
                view.Text = text;
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