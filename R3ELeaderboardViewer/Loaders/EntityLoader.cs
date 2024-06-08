using Android.Util;
using Plugin.CloudFirestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

#nullable enable

namespace R3ELeaderboardViewer.Firebase
{
    public class FirebaseCollectionAttribute : Attribute
    {
        public string? CollectionName { get; private set; }

        public FirebaseCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

        public FirebaseCollectionAttribute()
        {
            CollectionName = null;
        }
    }

    public class FirebaseParentAttribute : Attribute { }



    public class PropertyOrFieldInfo
    {
        private PropertyInfo? PropertyInfo { get; set; }
        private FieldInfo? FieldInfo { get; set; }

        public PropertyOrFieldInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        public PropertyOrFieldInfo(FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
        }

        public object GetValue(object obj)
        {
            return (PropertyInfo?.GetValue(obj) ?? FieldInfo?.GetValue(obj))!;
        }

        public void SetValue(object obj, object value)
        {
            // if value is int64, convert to int32, otherwise you get a cast exception
            if (value is long l)
            {
                value = (int)l;
            }
            PropertyInfo?.SetValue(obj, value);
            FieldInfo?.SetValue(obj, value);
        }

        public Type Type
        {
            get
            {
                return PropertyInfo?.PropertyType ?? FieldInfo.FieldType;
            }
        }

        public string Name
        {
            get
            {
                return PropertyInfo?.Name ?? FieldInfo.Name;
            }
        }
    }


    public class Entity : JsonEntity
    {
        public static readonly string TAG = "R3ELeaderboardViewer:" + typeof(Entity).Name;
        public static readonly Entity root = new Entity();

        public virtual IDocumentReference? Reference { get; set; }

        public Entity()
        {
            Reference = null;
        }

        public Task SaveFields()
        {
            return Reference!.SetAsync(ToDictionary());
        }

        public Task SaveSubCollections(bool recursive = false)
        {
            Log.Debug(TAG, "Saving subcollections for " + GetType().Name);
            var collections = Utils.GetAllPropertiesWith<FirebaseCollectionAttribute>(GetType());
            var tasks = new List<Task>();
            foreach (var collection in collections)
            {
                var value = collection.Item1.GetValue(this);
                if (value == null)
                {
                    continue;
                }
                Log.Debug(TAG, "Saving collection " + collection.Item1.Name);

                var typeInfo = Utils.TypeInfo(typeof(Entity), collection.Item1.Type);

                if (typeInfo.Item2 == Utils.TypeKind.Dictionary)
                {
                    var dict = (IDictionary)value;
                    foreach (var item in dict)
                    {
                        var entity = (Entity)item;
                        tasks.Add(entity.SaveFields());
                        if (recursive)
                        {
                            tasks.Add(entity.SaveSubCollections(true));
                        }
                    }
                }
                else if (typeInfo.Item2 == Utils.TypeKind.List)
                {
                    var list = (IList)value;
                    foreach (var item in list)
                    {
                        var entity = (Entity)item;
                        tasks.Add(entity.SaveFields());
                        if (recursive)
                        {
                            tasks.Add(entity.SaveSubCollections(true));
                        }
                    }
                }
            }
            return Task.WhenAll(tasks);
        }

        public async Task Save()
        {
            await SaveFields();
            await SaveSubCollections(true);
        }

        public Task Delete()
        {
            return Reference!.DeleteAsync();
        }

        public ICollectionReference Collection(string collectionName)
        {
            if (Reference == null)
            {
                return CrossCloudFirestore.Current.Instance.Collection(collectionName);
            }
            return Reference.Collection(collectionName);
        }

        private void LoadAsParent(Entity entity, PropertyOrFieldInfo property)
        {
            Log.Debug(TAG, "Loading parent " + property.Name);
            property.SetValue(entity, this);
        }

        private async Task LoadAsCollection(PropertyOrFieldInfo property, FirebaseCollectionAttribute collectionNameAttribute)
        {
            Log.Debug(TAG, "Loading collection " + property.Name);

            var type = property.Type;

            var entityTypeTuple = Utils.TypeInfo(typeof(Entity), type);
            var entityType = entityTypeTuple.Item1;
            var entityKind = entityTypeTuple.Item2;

            if (entityKind == Utils.TypeKind.Unknown || entityKind == Utils.TypeKind.Entity || entityType == null)
            {
                Log.Warn(TAG, "Field " + property.Name + " is not a collection of entities");
                return;
            }

            string collectionName = collectionNameAttribute.CollectionName ?? property.Name;
            var firebaseCollection = (await Collection(collectionName).GetAsync()).Documents;

            if (entityKind == Utils.TypeKind.Dictionary)
            {
                var dictionary = Utils.DictionaryOfType(typeof(string), entityType);
                foreach (var doc in firebaseCollection)
                {
                    dictionary.Add(doc.Id, await LoadChildEntity(entityType, doc));
                }
                Log.Debug(TAG, "Setting field " + property.Name + " to " + dictionary);
                property.SetValue(this, dictionary);
            }
            else
            {
                var entityList = Utils.ListOfType(entityType);

                foreach (var doc in firebaseCollection)
                {
                    entityList.Add(await LoadChildEntity(entityType, doc));
                }
                Log.Debug(TAG, "Setting field " + property.Name + " to " + entityList);
                property.SetValue(this, entityList);
            }
        }

        public async Task LoadChildEntity(Entity entity, IDocumentSnapshot doc)
        {
            var type = entity.GetType();

            var data = doc.Data ?? new Dictionary<string, object?>();

            entity.Reference = doc.Reference;

            var parents = Utils.GetAllPropertiesWith<FirebaseParentAttribute>(type);
            var collections = Utils.GetAllPropertiesWith<FirebaseCollectionAttribute>(type);

            entity.LoadJson(data!);

            foreach (var parent in parents)
            {
                LoadAsParent(entity, parent.Item1);
            }

            foreach (var collection in collections)
            {
                await entity.LoadAsCollection(collection.Item1, collection.Item2);
            }

            await entity.OnFirebasePopulate();
        }

        public async Task<T> LoadChildEntity<T>(IDocumentSnapshot doc) where T: Entity, new()
        {
            var entity = new T();
            await LoadChildEntity(entity, doc);
            return entity;
        }

        public async Task<Entity> LoadChildEntity(Type entityType, IDocumentSnapshot doc)
        {
            var entity = (Entity) Activator.CreateInstance(entityType);
            await LoadChildEntity(entity, doc);
            return entity;
        }

        public async Task LoadChildEntity(Entity entity)
        {
            await LoadChildEntity(entity, await LoadDocument(entity));
        }

        private async Task<IDocumentSnapshot> LoadDocument(Entity entity)
        {
            return await entity.Reference!.GetAsync();
        }

        protected virtual async Task OnFirebasePopulate() { }
    }
}