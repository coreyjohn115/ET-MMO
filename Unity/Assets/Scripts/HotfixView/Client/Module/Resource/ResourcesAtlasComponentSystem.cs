using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using YooAsset;

namespace ET.Client
{
    [EntitySystemOf(typeof (ResourcesAtlasComponent))]
    [FriendOf(typeof (ResourcesAtlasComponent))]
    public static partial class ResourcesAtlasComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ResourcesAtlasComponent self)
        {
            self.package = YooAssets.GetPackage(Define.DefPackageName);
            self.alwaysAtlas = new HashSet<string>() { "Icon_Common", "Widget" };
            if (self.alwaysAtlas != null)
            {
                foreach (string name in self.alwaysAtlas)
                {
                    self.LoadAsset(name.ToUISpriteAtlasPath()).NoContext();
                }
            }

            self.LoadAtlasConfig().NoContext();
        }

        [EntitySystem]
        private static void Destroy(this ResourcesAtlasComponent self)
        {
        }

        [Event(SceneType.Client)]
        private class CheckAssetEvent: AEvent<Scene, ClientHeart5>
        {
            protected override async ETTask Run(Scene scene, ClientHeart5 a)
            {
                await ETTask.CompletedTask;
                scene.GetComponent<ResourcesAtlasComponent>().Check();
            }
        }

        public static void OnWindowDispose(this ResourcesAtlasComponent self, Dictionary<string, int> refDict)
        {
            foreach (var pair in refDict)
            {
                string atlasName = self.atlasNameDict.GetValueOrDefault(pair.Key);
                if (self.refCountDict.TryGetValue(atlasName, out int _))
                {
                    self.refCountDict[atlasName] -= pair.Value;
                }
            }
        }

        public static async ETTask<Sprite> LoadSpriteAsync(this ResourcesAtlasComponent self, string name)
        {
            string atlasName = self.atlasNameDict.GetValueOrDefault(name);
            if (atlasName.IsNullOrEmpty())
            {
                if (name == UIComponent.DefIcon)
                {
                    return default;
                }

                Log.Error($"获取Sprite失败: {name}");
                return await self.LoadSpriteAsync(UIComponent.DefIcon);
            }

            if (!self.alwaysAtlas.Exists(atlasName))
            {
                if (!self.refCountDict.TryGetValue(atlasName, out int _))
                {
                    self.refCountDict.Add(atlasName, 1);
                }
                else
                {
                    self.refCountDict[atlasName]++;
                }
            }

            SpriteAtlas atlas = await self.LoadAsset(atlasName.ToUISpriteAtlasPath());
            return atlas.GetSprite(name);
        }

        private static async ETTask<SpriteAtlas> LoadAsset(this ResourcesAtlasComponent self, string location)
        {
            using CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>()
                    .Wait(CoroutineLockType.ResourcesLoader, location.GetHashCode());

            HandleBase handler;
            if (!self.handlers.TryGetValue(location, out handler))
            {
                handler = self.package.LoadAssetAsync<SpriteAtlas>(location);

                await handler.Task;

                self.handlers.Add(location, handler);
            }

            return ((AssetHandle)handler).AssetObject as SpriteAtlas;
        }

        private static void Check(this ResourcesAtlasComponent self)
        {
            using ListComponent<string> delList = ListComponent<string>.Create();
            foreach ((string atlasName, int count) in self.refCountDict)
            {
                if (count <= 0)
                {
                    delList.Add(atlasName);
                }
            }

            foreach (string s in delList)
            {
                self.refCountDict.Remove(s);
                if (self.handlers.Remove(s.ToUISpriteAtlasPath(), out HandleBase handler))
                {
                    Log.Info($"卸载图集: {s}");
                    ((AssetHandle)handler).Release();
                }
            }

            self.package.UnloadUnusedAssets();
        }

        private static async ETTask LoadAtlasConfig(this ResourcesAtlasComponent self)
        {
            AssetHandle operation = self.package.LoadAssetAsync("Assets/Bundles/Raw/SpriteToAtlas");
            await operation.Task;
            string s = operation.GetAssetObject<TextAsset>().text;
            var config = MongoHelper.FromJson<SpriteToAtlas>(s);
            for (int index = 0; index < config.NameList.Count; index++)
            {
                string name = config.NameList[index];
                foreach (string spriteName in config.AtlasList[index])
                {
                    self.atlasNameDict.Add(spriteName, name);
                }
            }

            operation.Release();
        }
    }
}