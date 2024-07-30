using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ChildOf]
    public class SkillUnit: Entity, IAwake, ISerializeToEntity
    {
        [BsonIgnore]
        public SkillConfig Config
        {
            get
            {
                int id = (int)this.Id * 1000 + this.level;
                return SkillConfigCategory.Instance.Get(id);
            }
        }

        [BsonIgnore]
        public SkillMasterConfig MasterConfig
        {
            get
            {
                return SkillMasterConfigCategory.Instance.Get((int)this.Id);
            }
        }

        /// <summary>
        /// 技能层数
        /// </summary>
        [BsonIgnore]
        public int Layer => this.layer;

        public int layer;
        public int level;

        public List<long> cdList = new List<long>();
    }
}