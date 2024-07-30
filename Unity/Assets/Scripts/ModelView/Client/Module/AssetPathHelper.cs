namespace ET.Client
{
    /// <summary>
    /// AB实用函数集，主要是路径拼接
    /// </summary>
    public static class AssetPathHelper
    {
        /// <summary>
        /// 获取ui路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ToUIPath(this string fileName)
        {
            var winCfg = WindowCategory.Instance.GetWindow(fileName);
            return $"Assets/Bundles/UI/Window/{winCfg.Path}.prefab";
        }

        /// <summary>
        /// 获取图集路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ToUISpriteAtlasPath(this string fileName)
        {
            return $"Assets/Bundles/UI/Atlas/{fileName}.spriteatlas";
        }

        /// <summary>
        /// 获取Buff特效路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ToBuffEffectPath(this string fileName)
        {
            return $"Assets/Bundles/Effect/Buff/{fileName}.prefab";
        }

        public static string ToUnitPath(this string unitName)
        {
            return $"Assets/Bundles/Unit/{unitName}/Unit.prefab";
        }
        
        public static string ToUnitWeaponPath(this string wName, string unitName)
        {
            return $"Assets/Bundles/Unit/{unitName}/{wName}.prefab";
        }
        
        public static string ToSoundPath(this string fileName)
        {
            return $"Assets/Bundles/Sound/{fileName}.mp3";
        }

        public static string ToUICommonPath(this string fileName)
        {
            return $"Assets/Bundles/UI/Common/{fileName}.prefab";
        }

        public static string ToUIHudPath(this string fileName)
        {
            return $"Assets/Bundles/UI/Common/Hud/{fileName}.prefab";
        }

        public static string ToUIItemPath(this string fileName)
        {
            return $"Assets/Bundles/UI/Item/{fileName}.prefab";
        }
        
        public static string ToComPath(this string fileName)
        {
            return $"Assets/Bundles/UI/Window/{fileName}.prefab";
        }

        public static string ToUnitHUDPath(this UnitType type)
        {
            switch (type)
            {
                case UnitType.Player:
                    return $"Assets/Bundles/UI/Common/Unit/PlayerHud.prefab";
                default:
                    return $"Assets/Bundles/UI/Common/Unit/PlayerHud.prefab";
            }
        }
    }
}