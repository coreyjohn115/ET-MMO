using System.Collections.Generic;
using YooAsset;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class ResourcesAtlasComponent: Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 常驻图集名称
        /// </summary>
        public HashSet<string> alwaysAtlas;

        public Dictionary<string, string> atlasNameDict = new();
        public Dictionary<string, HashSet<string>> loadAtlas = new();
        public ResourcePackage package;
        public Dictionary<string, HandleBase> handlers = new();

        /// <summary>
        /// 图集名称对应引用次数
        /// </summary>
        public Dictionary<string, int> refCountDict = new();
    }
}