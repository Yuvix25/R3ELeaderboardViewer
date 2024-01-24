using R3ELeaderboardViewer.Firebase;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Android.Content;

namespace R3ELeaderboardViewer
{
#nullable enable
    public static class Utils
    {
        public static void SetRequestedWith(WebClient wc)
        {
            wc.Headers.Set("X-Requested-With", "XMLHttpRequest");
        }

        public static int? ParseInt(object? obj)
        {
            if (obj == null)
            {
                return null;
            }
            
            if (obj is int n)
            {
                return n;
            }
            else
            {
                if (int.TryParse(obj.ToString(), out int i)) return i;
                return null;
            }
        }

        public static List<int?>? ParseIntList(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var list = (List<object>)obj;
            var intList = new List<int?>();
            foreach (var item in list)
            {
                intList.Add(ParseInt(item) ?? null);
            }
            return intList;
        }
        
        public static readonly Regex Whitespaces = new Regex(@"\s+", RegexOptions.Compiled);

        public static double? ParsePart(string? part, double? @default)
        {
            if (part == null)
            {
                return @default;
            }

            return double.Parse(part[..^1]);
        }
        public static double? ParseRaceRoomLaptime(string laptime, double? @default)
        {
            // 1h 2m 05.190s or 1m 02.190s or 1m 02s or 1m 02.1s or 1m 02.12s or 00.136s

            var parts = Whitespaces.Split(laptime, 2).Where(v => v.Length > 0).ToArray();
            if (parts.Length == 0)
            {
                return @default;
            }

            var partTime = ParsePart(parts[0], 0);
            switch (parts[0][^1])
            {
                case 'h':
                    partTime = partTime * 60 * 60;
                    break;
                case 'm':
                    partTime = partTime * 60;
                    break;
                case 's':
                    partTime = partTime; // for clarity
                    break;
                default:
                    return double.Parse(parts[0]);
            }
            if (parts.Length == 1)
            {
                return partTime;
            }
            return partTime + ParseRaceRoomLaptime(parts[1]);
        }

        public static double ParseRaceRoomLaptime(string laptime)
        {
            return ParseRaceRoomLaptime(laptime, 0) ?? 0;
        }

        public static string StringifyLaptime(double laptime, double? gap = null)
        {
            var minutes = (int)(laptime / 60);
            var seconds = laptime % 60;

            string res = $"{seconds:00.000}";
            if (minutes > 0)
            {
                res = $"{minutes}m {res}";
            }

            if (gap == null || gap == 0)
            {
                return res;
            }
            if (gap > 0)
            {
                return $"{res}, +{gap:00.000}s";
            }
            if (gap < 0)
            {
                return $"{res}, -{gap:00.000}s";
            }
            return res;
        }

        public static GameplayDifficulty ParseDifficulty(string difficulty)
        {
            switch (difficulty)
            {
                case "Novice":
                    return GameplayDifficulty.Novice;
                case "Amateur":
                    return GameplayDifficulty.Amateur;
                case "Get Real":
                    return GameplayDifficulty.GetReal;
                default:
                    return GameplayDifficulty.Unknown;
            }
        }

        private static float? scale;
        public static int DpToPixel(int dp, Context context)
        {
            scale ??= context.Resources.DisplayMetrics?.Density;
            return (int)(dp * (scale ?? 1));
        }

#nullable disable
        public static V GetOrDefault<K,V>(IDictionary<K, V>? dict, K key, V @default)
        {
            if (dict == null)
            {
                return @default;
            }
            dict.TryGetValue(key, out var value);
            return value ?? @default;
        }

        public static V GetOrDefault<K, V>(IDictionary<K, V>? dict, K key)
        {
            if (dict == null)
            {
                return default;
            }
            return GetOrDefault(dict, key, default);
        }

#nullable enable

        public static int GetIntOrDefault<K, V>(IDictionary<K, V>? dict, K key, int @default)
        {
            if (dict == null)
            {
                return @default;
            }
            dict.TryGetValue(key, out var value);
            return ParseInt(value) ?? @default;
        }

        public static int? GetIntOrDefault<K, V>(IDictionary<K, V>? dict, K key)
        {
            if (dict == null)
            {
                return null;
            }
            dict.TryGetValue(key, out var value);
            return ParseInt(value);
        }
    }
}