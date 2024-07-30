using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    /// <summary>
    /// 基础信息改变
    /// </summary>
    public struct BasicChangeEvent
    {
        public Unit Unit;
    }

    /// <summary>
    /// 玩家头像
    /// </summary>
    public struct UnitHead
    {
        public string HeadIcon;

        public int ChatFrame;

        public int ChatBubble;
    }

    /// <summary>
    /// 玩家基础数据
    /// </summary>
    [UnitCom]
    [ComponentOf(typeof (Unit))]
    public class UnitBasic: Entity, IAwake, ITransfer, ICache
    {
        /// <summary>
        /// 玩家数字ID
        /// </summary>
        public long Gid { get; set; }

        public string UserUid { get; set; }

        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [BsonIgnore]
        public int Level => this.level;

        public int level;

        /// <summary>
        /// vip等级
        /// </summary>
        public int VipLevel { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public UnitHead HeadIcon { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 总战力
        /// </summary>
        public long TotalFight { get; set; }
    }
}