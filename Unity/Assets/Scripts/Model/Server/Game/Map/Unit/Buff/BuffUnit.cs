using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    public struct BuffDyna
    {
        /// <summary>
        /// 天赋技能是否生效
        /// </summary>
        public bool IsEffect { get; set; }

        public EffectArgs BeEffectArgs { get; set; }

        public List<object> Args { get; init; }
    }

    /// <summary>
    /// Buff实体
    /// <para>Awake参数: Buff配置ID, 添加时间, 添加者Id</para>
    /// </summary>
    [ChildOf(typeof (BuffComponent))]
    public class BuffUnit: Entity, IAwake<int, long, long>, ISerializeToEntity
    {
        /// <summary>
        /// BuffId
        /// </summary>
        public int BuffId { get; set; }

        public int MasterId { get; set; }

        /// <summary>
        /// Buff过期时间
        /// </summary>
        public long ValidTime { get; set; }

        /// <summary>
        /// 当前Buff层级
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// Buff添加时间
        /// </summary>
        public long AddTime { get; set; }

        /// <summary>
        /// 上次脉冲触发时间
        /// </summary>
        public long LastUseTime { get; set; }

        /// <summary>
        /// 添加者UID
        /// </summary>
        public long AddRoleId { get; set; }

        /// <summary>
        /// 技能Id
        /// </summary>
        public int SkillId { get; set; }

        /// <summary>
        /// 移除标志
        /// </summary>
        public bool isRemove;
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<string, IBuffEffect> effectDict;

        /// <summary>
        /// 动态参数
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<string, BuffDyna> effectMap;

        //----------------------- 以下用于天赋动态修改的参数 ---------------------------

        /// <summary>
        /// Buff间隔时间
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Buff最大层级
        /// </summary>
        public int MaxLayer { get; set; }

        /// <summary>
        /// Buff视图
        /// </summary>
        public string ViewCmd { get; set; }

        /// <summary>
        /// buff持续时间
        /// </summary>
        public int Ms { get; set; }
    }
}