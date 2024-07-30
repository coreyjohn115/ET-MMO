using System;
using UnityEngine;
using UnityEngine.U2D;

namespace ET.Client
{
    public static class IconHelper
    {
        public static async ETTask SetSprite(Entity entity, ExtendImage image, string spriteName, AtlasType atlasType)
        {
            string path = entity.Scene().GetComponent<UIComponent>().GetAtlasPath(atlasType);
            Sprite sp = await entity.LoadSpriteAsync(path, spriteName);
            if (sp)
            {
                image.sprite = sp;
            }
            else
            {
                path = entity.Scene().GetComponent<UIComponent>().GetAtlasPath(AtlasType.Icon_Common);
                image.sprite = await entity.LoadSpriteAsync(path, UIComponent.DefIcon);
            }
        }

        public static async ETTask<Sprite> LoadIconSpriteAsync(this Entity self, string spriteName)
        {
            string path = self.Scene().GetComponent<UIComponent>().GetAtlasPath(AtlasType.Icon_Common);
            Sprite sp = await self.LoadSpriteAsync(path, spriteName);
            if (sp)
            {
                return sp;
            }

            return await self.LoadSpriteAsync(path, UIComponent.DefIcon);
        }

        public static async ETTask<Sprite> LoadWidgetSpriteAsync(this Entity self, string spriteName)
        {
            string path = self.Scene().GetComponent<UIComponent>().GetAtlasPath(AtlasType.Widget);
            Sprite sp = await self.LoadSpriteAsync(path, spriteName);
            if (sp)
            {
                return sp;
            }

            path = self.Scene().GetComponent<UIComponent>().GetAtlasPath(AtlasType.Icon_Common);
            return await self.LoadSpriteAsync(path, UIComponent.DefIcon);
        }

        public static async ETTask LoadAtlas(Entity entity, AtlasType atlasType)
        {
            string path = entity.Scene().GetComponent<UIComponent>().GetAtlasPath(atlasType);
            await entity.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<SpriteAtlas>(path);
        }

        /// <summary>
        /// 异步加载图集图片资源
        /// </summary>
        /// <OtherParam name="spriteName"></OtherParam>
        /// <returns></returns>
        public static async ETTask<Sprite> LoadSpriteAsync(this Entity self, string path, string spriteName)
        {
            try
            {
                SpriteAtlas spriteAtlas =
                        await self.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<SpriteAtlas>(path);
                Sprite sprite = spriteAtlas.GetSprite(spriteName);
                if (sprite == null)
                {
                    Log.Error($"sprite is null: {spriteName}");
                }

                return sprite;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
    }
}