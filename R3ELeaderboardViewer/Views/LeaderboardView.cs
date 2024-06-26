using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using R3ELeaderboardViewer.Firebase;
using R3ELeaderboardViewer.RaceRoom;
using System;
using System.Collections.Generic;


namespace R3ELeaderboardViewer.Views
{
    public class LeaderboardView : LinearLayout
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(LeaderboardView).Name;

        private List<EventHandler> ClickListeners = new List<EventHandler>();

        public void AddClickListener(EventHandler listener)
        {
            ClickListeners.Add(listener);
            Click += listener;
        }
        public void ClearClickListeners()
        {
            foreach (var listener in ClickListeners)
            {
                Click -= listener;
            }
            ClickListeners.Clear();
        }

        public LeaderboardView(Context context) : base(context)
        {
            Initialize();
        }

        public LeaderboardView(Context context, LeaderboardSnapshot snapshot) : base(context)
        {
            Initialize();

            LoadMinimizedLeaderboard(snapshot);
        }

        public LeaderboardView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public LeaderboardView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        public LeaderboardView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize();
        }

        private TableLayout TableView;
        private TextView NameView;
        private void Initialize()
        {
            LayoutInflater.From(Context).Inflate(Resource.Layout.leaderboard_view, this);
            Orientation = Orientation.Vertical;

            TableView = FindViewById<TableLayout>(Resource.Id.leaderboard_entries);

            NameView = FindViewById<TextView>(Resource.Id.leaderboard_name);
        }


        private LeaderboardEntryView AddEntry(LeaderboardEntry entry, int position)
        {
            LeaderboardEntryView view;
            if (position < TableView.ChildCount)
            {
                view = (LeaderboardEntryView)TableView.GetChildAt(position);
            }
            else
            {
                view = new LeaderboardEntryView(Context);
            }
            var attrs = view.LoadFromLeadeboardEntry(entry);
            for (var i = 0; i < attrs.Length; i++) {
                switch (attrs[i])
                {
                    case "Team":
                    case "ProfilePicture":
                        TableView.SetColumnShrinkable(i, true);
                        break;
                }
            }
            view.SetBackgroundColor(Android.Graphics.Color.Transparent);
            view.SetBackgroundResource(0);

            if (position >= TableView.ChildCount)
            {
                TableView.AddView(view);
            }

            return view;
        }

        public int EntryCount => TableView.ChildCount;

        private void CropEntries(int count)
        {
            if (TableView.ChildCount > count)
            {
                TableView.RemoveViews(count, TableView.ChildCount - count);
            }
        }

        /// <summary>
        /// Load a minimized leaderboard, showing only the user's position, the entries around it and the first entry.
        /// </summary>
        public void LoadMinimizedLeaderboard(LeaderboardSnapshot snapshot)
        {
            NameView.Text = snapshot.Parent.LeaderboardName;

            var showPosition = snapshot.UserIndex ?? 0;
            var start = Math.Max(1, showPosition - RaceRoomApiManager.MINIMUM_SPACE_AHEAD + 1);
            var end = Math.Min(snapshot.Entries.Count, showPosition + RaceRoomApiManager.MINIMUM_SPACE_BEHIND + 1);

            TableView.RemoveAllViews();
            CropEntries(end - start);

            LeaderboardEntryView me = null;
            if (snapshot.Entries.Count > 0)
            {
                var view = AddEntry(snapshot.Entries[0], 0);
                view.SetBackgroundResource(Resource.Drawable.bottom_border);
                if (snapshot.UserIndex == 0)
                {
                    me = view;
                }
            }
            for (var i = start; i < end; i++)
            {
                var entryView = AddEntry(snapshot.Entries[i], i);
                
                if (i == snapshot.UserIndex)
                {
                    me = entryView;
                }
            }

            MarkMe(me);
        }

        public void LoadFullLeaderboard(LeaderboardSnapshot snapshot)
        {
            CropEntries(snapshot.Entries.Count);
            NameView.Text = snapshot.Parent.LeaderboardName;

            Log.Debug(TAG, "Started loading full leaderboard");

            for (var i = 0; i < snapshot.Entries.Count; i++)
            {
                var view = AddEntry(snapshot.Entries[i], i);
                if (i == snapshot.UserIndex)
                {
                    MarkMe(view);
                }
            }
        }

        public LeaderboardEntryView Me { get; private set; } = null;
        private void MarkMe(LeaderboardEntryView view)
        {
            if (view == null)
            {
                return;
            }
            Me = view;
            if (view.Position == 1)
            {
                view.SetBackgroundResource(Resource.Drawable.bottom_border_highlighted);
            } else
            {
                view.SetBackgroundColor(Android.Graphics.Color.Rgb(255, 238, 173));
            }
        }
    }
}