using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof (ClientSkillComponent))]
    public class ClientSkillUnit: Entity, IAwake
    {
        public SkillConfig Config
        {
            get
            {
                int id = (int)this.Id * 1000 + this.level;
                return SkillConfigCategory.Instance.Get(id);
            }
        }

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
        public int Layer => this.layer;

        public int layer;
        public int level;

        public List<long> cdList = new List<long>();
    }
}