using System.IO;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (DataSaveComponent))]
    [FriendOf(typeof (DataSaveComponent))]
    public static partial class DataSaveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this DataSaveComponent self)
        {
            self.rootPath = $"{Application.persistentDataPath}/Cache/Data/";
            if (!Directory.Exists(self.rootPath))
            {
                Directory.CreateDirectory(self.rootPath);
            }

            Log.Debug(self.rootPath);
        }

        public static string GetString(this DataSaveComponent self, string key)
        {
            return PlayerPrefs.GetString(key, string.Empty);
        }

        public static void SaveString(this DataSaveComponent self, string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public static void Save<T>(this DataSaveComponent self, string key, T value) where T : Entity
        {
            var path = $"{self.rootPath}{key}.dat";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, value.ToBson());
        }

        public static T Get<T>(this DataSaveComponent self, string key) where T : Entity
        {
            var path = $"{self.rootPath}{key}.dat";
            if (!File.Exists(path))
            {
                return default;
            }

            var bytes = File.ReadAllBytes(path);
            var entity = (Entity) MongoHelper.Deserialize(typeof (T), bytes);
            return entity as T;
        }

        public static async ETTask SaveAsync<T>(this DataSaveComponent self, string key, T value) where T : Entity
        {
            var path = $"{self.rootPath}{key}.dat";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            await File.WriteAllBytesAsync(path, value.ToBson());
        }

        public static async ETTask<T> GetAsync<T>(this DataSaveComponent self, string key) where T : Entity
        {
            var path = $"{self.rootPath}{key}.dat";
            if (!File.Exists(path))
            {
                return default;
            }

            var bytes = await File.ReadAllBytesAsync(path);
            var entity = (Entity) MongoHelper.Deserialize(typeof (T), bytes);
            return entity as T;
        }
    }
}