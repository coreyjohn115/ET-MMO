namespace ET.Server
{
    /// <summary>
    /// 添加技能
    /// 技能ID;
    /// </summary>
    [Buff(EffectName)]
    public class AddSkillEffect: ABuffEffect
    {
        public const string EffectName = "AddSkill";

        protected override void OnCreate(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
        }

        protected override void OnRemove(BuffComponent sel, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgf, object[] args)
        {
        }
    }
}