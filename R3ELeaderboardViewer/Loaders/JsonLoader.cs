#nullable enable

using System;
using System.Collections.Generic;

namespace R3ELeaderboardViewer.Firebase
{
    public class FirebaseFieldAttribute : Attribute
    {
        public string? FieldName { get; private set; }

        public FirebaseFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public FirebaseFieldAttribute()
        {
            FieldName = null;
        }
    }

    public class JsonEntity
    {
        public void LoadJson(IDictionary<string, object> data)
        {
            var fields = Utils.GetAllPropertiesWith<FirebaseFieldAttribute>(GetType());
            foreach (var field in fields)
            {
                var attribute = field.Item2;
                var fieldName = (field.Item1.Name ?? attribute.FieldName)!;
                if (data.ContainsKey(fieldName))
                {
                    var typeInfo = Utils.TypeInfo(typeof(JsonEntity), field.Item1.Type);

                    var type = typeInfo.Item1;
                    var typeKind = typeInfo.Item2;

                    if (typeKind == Utils.TypeKind.Unknown || type == null)
                    {
                        field.Item1.SetValue(this, data[fieldName]);
                    }
                    else if (typeKind == Utils.TypeKind.List)
                    {
                        var list = (IList<object>)data[fieldName]!;
                        var listInstance = Utils.ListOfType(type);
                        for (int i = 0; i < list.Count; i++)
                        {
                            var item = list[i];
                            listInstance.Add(LoadNew(type, (Dictionary<string, object>)item!));
                        }
                        field.Item1.SetValue(this, listInstance);
                    }
                    else if (typeKind == Utils.TypeKind.Dictionary)
                    {
                        var dict = (IDictionary<string, object>)data[fieldName]!;
                        var dictInstance = Utils.DictionaryOfType(typeof(string), type);
                        foreach (var key in dict.Keys)
                        {
                            dictInstance[key] = LoadNew(type, (Dictionary<string, object>)dict[key]!);
                        }
                        field.Item1.SetValue(this, dictInstance);
                    }
                    else if (typeKind == Utils.TypeKind.Entity)
                    {
                        field.Item1.SetValue(this, LoadNew(type, (Dictionary<string, object>)data[fieldName]!));
                    }   
                }
            }
        }

        public static JsonEntity LoadNew(Type type, Dictionary<string, object> data)
        {
            var obj = (JsonEntity)Activator.CreateInstance(type)!;
            obj.LoadJson(data);
            return obj;
        }

        public Dictionary<string, object> ToDictionary()
        {
            var fields = Utils.GetAllPropertiesWith<FirebaseFieldAttribute>(GetType());
            var dict = new Dictionary<string, object>();
            foreach (var field in fields)
            {
                var attribute = field.Item2;
                var fieldName = (field.Item1.Name ?? attribute.FieldName)!;
                var value = field.Item1.GetValue(this);
                if (value != null)
                {
                    var typeInfo = Utils.TypeInfo(typeof(JsonEntity), field.Item1.Type);

                    var type = typeInfo.Item1;
                    var typeKind = typeInfo.Item2;

                    if (typeKind == Utils.TypeKind.Unknown || type == null)
                    {
                        dict[fieldName] = value;
                    }
                    else if (typeKind == Utils.TypeKind.List)
                    {
                        var list = Utils.CastToList<JsonEntity>(type, value);
                        var listInstance = new List<object>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            listInstance.Add(list[i].ToDictionary());
                        }
                        dict[fieldName] = listInstance;
                    }
                    else if (typeKind == Utils.TypeKind.Dictionary)
                    {
                        var dictValue = Utils.CastToDictionary<JsonEntity>(type, value);
                        var dictInstance = new Dictionary<string, object>();
                        foreach (var key in dictValue.Keys)
                        {
                            dictInstance[key] = dictValue[key].ToDictionary();
                        }
                        dict[fieldName] = dictInstance;
                    }
                    else if (typeKind == Utils.TypeKind.Entity)
                    {
                        dict[fieldName] = ((JsonEntity)value!).ToDictionary();
                    }
                }
            }
            return dict;
        }
    }
}