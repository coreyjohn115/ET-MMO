using System.IO;
using YooAsset.Editor;

namespace ET.Client
{
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