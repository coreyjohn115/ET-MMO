using System.Collections.Generic;

namespace ET.Client
{
    public struct ClientUseSkill
    {
        public Unit Unit;
        public int MasterId;
    }

    public struct PopHurtHud
    {
        public long Id;
        public Unit Dest;
        public HurtProto Info;
    }

    [ComponentOf(typeof (Unit))]
    public class ClientSkillComponent: Entity, IAwake
    {
        /// <summary>
        /// 是否正在使用技能
        /// </summary>
        public bool useSkill = false;
        public long lastAttackTime;
        public int index;

        // 普攻列表
        public List<int> normalSkillList = new List<int>();

        public List<int> skillList = new List<int>();
    }
}