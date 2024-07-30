using System.IO;
using YooAsset.Editor;

namespace ET.Client
{
    [DisplayName("收集图集")]
    public class CollectAtlas: IFilterRule
    {
        public bool IsCollectAsset(FilterRuleData data)
        {
            return Path.GetExtension(data.AssetPath) == ".spriteatlas";
        }
    }
}