using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public enum ItemType
    {
        /// <summary>
        /// 资源道具
        /// </summary>
        Resource = 10,

        /// <summary>
        /// 普通道具
        /// </summary>
        Normal = 11,

        /// <summary>
        /// 装备
        /// </summary>
        Equip = 12,

        /// <summary>
        /// 宠物
        /// </summary>
        Pet = 13,
    }

    [ChildOf]
    public class ItemData: Entity, IAwake<int>, IDestroy, ISerializeToEntity
    {
        /// <summary>
        /// 道具数量
        /// </summary>
        public long Count { get; set; }

        public long ValidTime { get; set; }

        public int cfgId;

        /// <summary>
        /// 是否绑定
        /// </summary>
        public bool Bind { get; set; }

        [BsonIgnore]
        public ItemType ItemType => (ItemType)this.Config.Type;

        [BsonIgnore]
        public IItemConfig Config
        {
            get
            {
                return XItemConfigCategory.Instance.GetConfig(this.cfgId);
            }
        }
    }
}