using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using R3ELeaderboardViewer.Firebase;
using R3ELeaderboardViewer.Fragments;
using R3ELeaderboardViewer.Views;
using System.Collections.Generic;
using System.Linq;

namespace R3ELeaderboardViewer.Adapters
{
    public class LeaderboardExpandableListViewAdapter : BaseExpandableListAdapter
    {
        static readonly string TAG = "LeaderboardExpandableListViewAdaptor:" + typeof(MainFragment).Name;

        private Context context;
        private List<string> leaderboardGroups;
        private Dictionary<string, LeaderboardSnapshot> leaderboardSnapshots;
        private Dictionary<string, List<string>> structure;
        private LayoutInflater inflater;

        public delegate void LeaderboardClickDelegate(LeaderboardSnapshot snapshot);
        public event LeaderboardClickDelegate OnLeaderboardClick = delegate { };

        public LeaderboardExpandableListViewAdapter(Context context, Dictionary<string, List<LeaderboardSnapshot>> structure)
        {
            this.context = context;
            
            UpdateData(structure);

            inflater = LayoutInflater.From(context);
        }

        public void UpdateData(Dictionary<string, List<LeaderboardSnapshot>> structure)
        {
            leaderboardGroups = structure.Keys.ToList();
            leaderboardSnapshots = new Dictionary<string, LeaderboardSnapshot>();
            this.structure = new Dictionary<string, List<string>>();
            foreach (var header in leaderboardGroups)
            {
                List<string> snapshotIds = new List<string>();
                foreach (var snapshot in structure[header])
                {
                    leaderboardSnapshots.Add(snapshot.Parent.FirebaseId, snapshot);
                    snapshotIds.Add(snapshot.Parent.FirebaseId);
                }
                this.structure.Add(header, snapshotIds);
            }
        }

        public override int GroupCount
        {
            get
            {
                return leaderboardGroups.Count;
            }
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return structure[leaderboardGroups[groupPosition]].Count;
        }

        public override Object GetGroup(int groupPosition)
        {
            return leaderboardGroups[groupPosition];
        }

        public override Object GetChild(int groupPosition, int childPosition)
        {
            return structure[leaderboardGroups[groupPosition]][childPosition];
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override bool HasStableIds
        {
            get
            {
                return false;
            }
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            string headerTitle = (string)GetGroup(groupPosition);

            TextView header;
            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.leaderboard_elv_header, null);
                header = convertView.FindViewById<TextView>(Resource.Id.leaderboard_elv_header_text);
                convertView.Tag = header;
            }
            else
            {
                header = (TextView)convertView.Tag;
            }
            header.Text = headerTitle;

            return convertView;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            string childId = (string)GetChild(groupPosition, childPosition);

            LeaderboardView view;
            if (convertView == null)
            {
                view = new LeaderboardView(context);
            } else
            {
                view = (LeaderboardView)convertView;
            }

            view.LoadMinimizedLeaderboard(leaderboardSnapshots[childId]);
            view.ClearClickListeners();
            view.AddClickListener((sender, args) =>
            {
                OnLeaderboardClick?.Invoke(leaderboardSnapshots[childId]);
            });

            return view;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}