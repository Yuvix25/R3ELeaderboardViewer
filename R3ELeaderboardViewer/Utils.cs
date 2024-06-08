using R3ELeaderboardViewer.Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Android.Content;
using Android.Util;
using Android.Graphics.Drawables;
using Android.Support.V4.Content.Res;
using System.Reflection;
using System.Collections;

namespace R3ELeaderboardViewer
{
#nullable enable
    public static class Utils
    {
        static readonly string TAG = "R3ELeaderboardViewer:" + typeof(Utils).Name;


        public class CountryFlags
        {
            private static readonly string[] CountryCodes = new string[]
            {
                "ad", "ae", "af", "ag", "ai", "al", "am", "ao", "aq", "ar", "as", "at", "au", "aw", "ax",
                "az", "ba", "bb", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bl", "bm", "bn", "bo",
                "bq", "br", "bs", "bt", "bv", "bw", "by", "bz", "ca", "cc", "cd", "cf", "cg", "ch",
                "ci", "ck", "cl", "cm", "cn", "co", "cr", "cu", "cv", "cw", "cx", "cy", "cz", "de",
                "dj", "dk", "dm", "do", "dz", "ec", "ee", "eg", "eh", "er", "es", "et", "fi", "fj",
                "fk", "fm", "fo", "fr", "ga", "gb", "gd", "ge", "gf", "gg", "gh", "gi", "gl", "gm",
                "gn", "gp", "gq", "gr", "gs", "gt", "gu", "gw", "gy", "hk", "hm", "hn", "hr", "ht",
                "hu", "id", "ie", "il", "im", "in", "io", "iq", "ir", "is", "it", "je", "jm", "jo",
                "jp", "ke", "kg", "kh", "ki", "km", "kn", "kp", "kr", "kw", "ky", "kz", "la", "lb",
                "lc", "li", "lk", "lr", "ls", "lt", "lu", "lv", "ly", "ma", "mc", "md", "me", "mf",
                "mg", "mh", "mk", "ml", "mm", "mn", "mo", "mp", "mq", "mr", "ms", "mt", "mu", "mv",
                "mw", "mx", "my", "mz", "na", "nc"
            };
            public static Dictionary<string, Drawable> CountryFlagDrawables = new Dictionary<string, Drawable>();

            public static void LoadCountryFlags(Context context)
            {
                foreach (var countryCode in CountryCodes)
                {
                    GetCountryFlagDrawable(context, countryCode);
                }
            }

            public static Drawable? GetCountryFlagDrawable(Context context, string? countryCode)
            {
                if (countryCode == null)
                {
                    return null;
                }
                if (CountryFlagDrawables.ContainsKey(countryCode))
                {
                    return CountryFlagDrawables[countryCode];
                }
                var id = context.Resources?.GetIdentifier(countryCode?.ToLower(), "drawable", context.PackageName);
                if (id == null || id == 0)
                {
                    Log.Debug(TAG, $"Could not find drawable for country code {countryCode}");
                    return null;
                }
                var res = ResourcesCompat.GetDrawable(context.Resources, (int)id, null);
                if (res == null)
                {
                    Log.Debug(TAG, $"Could not find drawable for country code {countryCode}");
                    return null;
                }

                CountryFlagDrawables[countryCode!] = res;
                return res;
            }
        }


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
                intList.Add(ParseInt(item));
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

        public static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            return (T)memberInfo.GetCustomAttribute(typeof(T));
        }

        public static List<Tuple<PropertyOrFieldInfo, T>> GetAllPropertiesWith<T>(Type type) where T : Attribute
        {
            var properties = new List<Tuple<PropertyOrFieldInfo, T>>();
            foreach (var property in type.GetProperties())
            {
                var attr = GetAttribute<T>(property);
                if (attr != null)
                {
                    properties.Add(new Tuple<PropertyOrFieldInfo, T>(new PropertyOrFieldInfo(property), attr));
                }
            }
            foreach (var field in type.GetFields())
            {
                var attr = GetAttribute<T>(field);
                if (attr != null)
                {
                    properties.Add(new Tuple<PropertyOrFieldInfo, T>(new PropertyOrFieldInfo(field), attr));
                }
            }
            return properties;
        }

        public static IList ListOfType(Type type)
        {
            var constructedListType = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(constructedListType);
        }

        public static IDictionary DictionaryOfType(Type keyType, Type valueType)
        {
            var constructedDictType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            return (IDictionary)Activator.CreateInstance(constructedDictType);
        }


        public enum TypeKind
        {
            Unknown,
            Entity,
            List,
            Dictionary
        }

        public static Tuple<Type?, TypeKind> TypeInfo(Type baseType, Type type)
        {
            Type? entityType;
            TypeKind kind;
            // check if field is dictionary or collection of entities
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                kind = TypeKind.Dictionary;
                entityType = type.GetGenericArguments()[1];
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                kind = TypeKind.List;
                entityType = type.GetGenericArguments()[0];
            }
            else if (type.IsSubclassOf(baseType))
            {
                kind = TypeKind.Entity;
                entityType = type;
            }
            else
            {
                return new Tuple<Type?, TypeKind>(null, TypeKind.Unknown);
            }

            if (!entityType.IsSubclassOf(baseType))
            {
                return new Tuple<Type?, TypeKind>(null, TypeKind.Unknown);
            }

            return new Tuple<Type?, TypeKind>(entityType, kind);
        }


        /*
         * Casts a List<F> to a List<T> where F is a subclass of T
         */
        public static List<T> ListCast<F, T>(object obj) where F : T
        {
            var list = (List<F>)obj;
            var res = new List<T>();
            foreach (var item in list)
            {
                res.Add(item);
            }
            return res;
        }

        /*
         * Casts a Dictionary<string, F> to a Dictionary<string, T> where F is a subclass of T
         */
        public static Dictionary<string, T> DictionaryCast<F, T>(object obj) where F : T
        {
            var dict = (Dictionary<string, F>)obj;
            var res = new Dictionary<string, T>();
            foreach (var key in dict.Keys)
            {
                res[key] = dict[key];
            }
            return res;
        }

        /*
         * Casts a List<type> to a List<T>
         */
        public static List<T> CastToList<T>(Type type, object obj)
        {
            var method = typeof(Utils).GetMethod("ListCast", new Type[] { typeof(object) });
            var genericMethod = method.MakeGenericMethod(type, typeof(T));
            object res;
            try
            {
                res = genericMethod.Invoke(null, new object[] { obj })!;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to cast list", e);
            }
            return (List<T>)res;
        }

        /*
         * Casts a Dictionary<string, type> to a Dictionary<string, T>
         */
        public static Dictionary<string, T> CastToDictionary<T>(Type type, object obj)
        {
            var method = typeof(Utils).GetMethod("DictionaryCast", new Type[] { typeof(object) });
            var genericMethod = method.MakeGenericMethod(type, typeof(T));
            object res;
            try
            {
                res = genericMethod.Invoke(null, new object[] { obj })!;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to cast dictionary", e);
            }
            return (Dictionary<string, T>)res;
        }

        public static string? GetOrdinalSuffix(int num)
        {
            string number = num.ToString();
            if (number.EndsWith("11")) return "th";
            if (number.EndsWith("12")) return "th";
            if (number.EndsWith("13")) return "th";
            if (number.EndsWith("1")) return "st";
            if (number.EndsWith("2")) return "nd";
            if (number.EndsWith("3")) return "rd";
            return "th";
        }

        public static string GetWithOrdinalSuffix(int? num)
        {
            if (!num.HasValue)
            {
                return "(unknown)";
            }
            return num + GetOrdinalSuffix(num.Value);
        }
    }
}