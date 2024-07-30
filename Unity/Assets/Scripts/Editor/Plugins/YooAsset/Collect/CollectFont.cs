using System.IO;
using YooAsset.Editor;

namespace ET.Client
{
    [DisplayName("收集字体")]
    public class CollectFont: IFilterRule
    {
        public bool IsCollectAsset(FilterRuleData data)
        {
            var ext = Path.GetExtension(data.AssetPath);
            if (ext == ".ttf" || ext == ".fontsettings")
            {
                return true;
            }

            return false;
        }
    }
}