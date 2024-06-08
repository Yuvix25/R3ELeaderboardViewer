using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using R3ELeaderboardViewer.Firebase;
using System;
using System.Collections.Generic;

namespace R3ELeaderboardViewer
{
    public class Notifications
    {
        public static readonly string TAG = "R3ELeaderboardViewer:" + typeof(Notifications).Name;
        public static readonly string FOREGROUND_CHANNEL_ID = "r3e_leaderboard_viewer_foreground_channel";
        public static readonly string FOREGROUND_CHANNEL_NAME = "Leaderboard Viewer Service";
        public static readonly string FOREGROUND_CHANNEL_DESCRIPTION = "Service for querying leaderboards and sending notifications";
        public static readonly string CHANNEL_ID = "r3e_leaderboard_viewer_channel";
        public static readonly string CHANNEL_NAME = "Leaderboard Notifications";
        public static readonly string CHANNEL_DESCRIPTION = "Notifications for new leaderboard entries, changes in leaderboard position, upcoming end of competitions, etc.";
        

        private static ContextWrapper ContextWrapper;

        public static void Initialize(ContextWrapper contextWrapper)
        {
            ContextWrapper = contextWrapper;
            CreateNotificationChannel();
        }

        private static void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var notificationManager = (NotificationManager)ContextWrapper.GetSystemService(Context.NotificationService);

            var foregroundChannel = new NotificationChannel(FOREGROUND_CHANNEL_ID, FOREGROUND_CHANNEL_NAME, NotificationImportance.Min)
            {
                Description = FOREGROUND_CHANNEL_DESCRIPTION
            };
            foregroundChannel.SetSound(null, null);

            var channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.Default)
            {
                Description = CHANNEL_DESCRIPTION
            };

            notificationManager.CreateNotificationChannel(foregroundChannel);
            notificationManager.CreateNotificationChannel(channel);
        }


        private static int HashCode(params object?[] objects)
        {
            int hash = 17;
            foreach (var obj in objects)
            {
                hash = hash * 31 + (obj?.GetHashCode() ?? 0);
            }
            return hash;
        }

        private static int NotificationHash(Notification notification)
        {
            // create hash from notification content
            var title = notification.Extras.GetString(Notification.ExtraTitle);
            var text = notification.Extras.GetString(Notification.ExtraText);
            var subText = notification.Extras.GetString(Notification.ExtraSubText);
            var bigText = notification.Extras.GetString(Notification.ExtraBigText);
            return HashCode(title, text, subText, bigText);
        }

        private static Notification.Builder Builder()
        {
            return new Notification.Builder(ContextWrapper, CHANNEL_ID)
                .SetSmallIcon(Resource.Mipmap.ic_launcher_foreground)
                .SetColor(ContextWrapper.GetColor(Resource.Color.colorAccent))
                .SetAutoCancel(true);
        }

        public static void CreateNotificationForeground(string title, string content, PendingIntent pendingIntent)
        {
            var notification = Builder()
                .SetContentTitle(title)
                .SetContentText(content)
                .SetContentIntent(pendingIntent)
                .Build();

            Log.Debug(TAG, "Starting foreground service with notification");

            ((Service)ContextWrapper).StartForeground(1, notification);

            Log.Debug(TAG, "Foreground service started");
        }

        public static void CreateNotification(string title, string content)
        {
            var notificationManager = (NotificationManager)ContextWrapper.GetSystemService(Context.NotificationService);
            var notification = Builder()
                .SetContentTitle(title)
                .SetContentText(content)
                .Build();

            notificationManager.Notify(NotificationHash(notification), notification);
        }

        public static void TestNotification()
        {
            CreateNotification("Test notification", "This is a test notification");
        }



        private static void NewCompetitionNotification(Leaderboard comp)
        {
            if (comp.Snapshots.Count == 0)
            {
                return;
            }
            Log.Info(TAG, "New competition notification for " + comp.LeaderboardName);
            var position = Utils.GetWithOrdinalSuffix(comp.Snapshots[^1].UserIndex + 1);
            CreateNotification(comp.LeaderboardName, "New entrance. You are currently in " + position + " place.");
        }

        private static void UpdatedCompetitionNotification(Leaderboard comp)
        {
            if (comp.Snapshots.Count < 2)
            {
                return;
            }
            var oldPosition = comp.Snapshots[^2].UserIndex + 1;
            var newPosition = comp.Snapshots[^1].UserIndex + 1;
            if (oldPosition == newPosition)
            {
                return;
            }
            Log.Info(TAG, "Updated competition notification for " + comp.LeaderboardName);
            var oldPositionOrdinal = Utils.GetWithOrdinalSuffix(comp.Snapshots[^2].UserIndex + 1);
            var newPositionOrdinal = Utils.GetWithOrdinalSuffix(comp.Snapshots[^1].UserIndex + 1);
            CreateNotification(comp.LeaderboardName, "You moved from " + oldPositionOrdinal + " to " + newPositionOrdinal + " place.");
        }

        private static void CompetitionEndedNotification(Leaderboard comp)
        {
            if (comp.Snapshots.Count == 0)
            {
                return;
            }
            Log.Info(TAG, "Competition ended notification for " + comp.LeaderboardName);
            var position = Utils.GetWithOrdinalSuffix(comp.Snapshots[^1].UserIndex + 1);
            CreateNotification(comp.LeaderboardName, "Competition has ended. You finished in " + position + " place.");
        }


        public static void CreateNotificationsForCompetitions(UserData userData)
        {
            if (userData == null || !userData.LatestRecentCompetitionsResult.HasValue)
            {
                Log.Info(TAG, "No recent competitions to notify about");
                return;
            }
            var newComps = userData.LatestRecentCompetitionsResult.Value.NewCompetitions;
            var existingComps = userData.LatestRecentCompetitionsResult.Value.ExistingCompetitions;
            var expiredComps = userData.LatestRecentCompetitionsResult.Value.ExpiredCompetitions;

            var ended = new HashSet<int>();
            foreach (var comp in newComps)
            {
                NewCompetitionNotification(comp);
                if (comp.EndDate.Value.ToDateTime() < DateTime.Now)
                {
                    CompetitionEndedNotification(comp);
                    ended.Add(comp.LeaderboardId);
                }
            }
            foreach (var comp in existingComps)
            {
                UpdatedCompetitionNotification(comp);
                if (comp.EndDate.Value.ToDateTime() < DateTime.Now)
                {
                    CompetitionEndedNotification(comp);
                    ended.Add(comp.LeaderboardId);
                }
            }
            foreach (var comp in expiredComps)
            {
                if (ended.Contains(comp.LeaderboardId))
                {
                    continue;
                }
                CompetitionEndedNotification(comp);
            }
        }
    }
}