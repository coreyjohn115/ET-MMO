using System;
using System.IO;
using YooAsset.Editor;

namespace ET.Client
{
    [DisplayName("打包特效纹理（自定义）")]
    public class PackEffectTexture: IPackRule
    {
        private const string PackDirectory = "Assets/Effect/Textures/";

        PackRuleResult IPackRule.GetPackRuleResult(PackRuleData data)
        {
            string assetPath = data.AssetPath;
            if (assetPath.StartsWith(PackDirectory) == false)
                throw new Exception($"Only support folder : {PackDirectory}");

            string assetName = Path.GetFileName(assetPath).ToLower();
            string firstChar = assetName.Substring(0, 1);
            string bundleName = $"{PackDirectory}effect_texture_{firstChar}";
            var packRuleResult = new PackRuleResult(bundleName, DefaultPackRule.AssetBundleFileExtension);
            return packRuleResult;
        }
    }

    [DisplayName("打包视频（自定义）")]
    public class PackVideo: IPackRule
    {
        public PackRuleResult GetPackRuleResult(PackRuleData data)
        {
            string bundleName = RemoveExtension(data.AssetPath);
            string fileExtension = Path.GetExtension(data.AssetPath);
            fileExtension = fileExtension.Remove(0, 1);
            PackRuleResult result = new PackRuleResult(bundleName, fileExtension);
            return result;
        }

        private string RemoveExtension(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            int index = str.LastIndexOf(".", StringComparison.Ordinal);
            if (index == -1)
                return str;
            else
                return str.Remove(index); //"assets/config/test.unity3d" --> "assets/config/test"
        }
    }

    [DisplayName("资源包名: Spine打包规则")]
    public class PackSpineRule: IPackRule
    {
        public PackRuleResult GetPackRuleResult(PackRuleData data)
        {
            if (data.AssetPath.EndsWith(".prefab"))
            {
                var bundleName = PathHelper.RemoveExtension(data.AssetPath);
                return new PackRuleResult(bundleName, DefaultPackRule.AssetBundleFileExtension);
            }
            else
            {
                string bundleName = Path.GetDirectoryName(data.AssetPath);
                return new PackRuleResult(bundleName, DefaultPackRule.AssetBundleFileExtension);
            }
        }
    }

    /// <summary>
    /// 以分组名称作为资源包名
    /// </summary>
    [DisplayName("资源包名: PackData指定")]
    public class PackUserData: IPackRule
    {
        PackRuleResult IPackRule.GetPackRuleResult(PackRuleData data)
        {
            PackRuleResult result = new(data.UserData, DefaultPackRule.AssetBundleFileExtension);
            return result;
        }
    }

    [DisplayName("资源包名: 单图集AB显式包含精灵")]
    public class PackAtlasWithTexture: IPackRule
    {
        public PackRuleResult GetPackRuleResult(PackRuleData data)
        {
            var bundleName = PathHelper.RemoveExtension(data.AssetPath);
            return new PackRuleResult(bundleName, DefaultPackRule.AssetBundleFileExtension);
        }

        public bool IsPackSprite() => true;
    }

    [DisplayName("资源包名: 图集每张散图分别打包")]
    public class PackAtlasTexSeparately: IPackRule
    {
        public PackRuleResult GetPackRuleResult(PackRuleData data)
        {
            var bundleName = PathHelper.RemoveExtension(data.AssetPath);
            return new PackRuleResult(bundleName, DefaultPackRule.AssetBundleFileExtension);
        }

        public bool IsPackSprite() => false;
    }
}