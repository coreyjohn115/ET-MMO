namespace ET.Server
{
    /// <summary>
    /// 添加技能
    /// 技能ID;
    /// </summary>
    [Buff("AddSkill")]
    public class AddSkillEffect: ABuffEffect
    {
        protected override void OnCreate(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
        }

        protected override void OnRemove(BuffComponent sel, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgf, object[] args)
        {
        }
    }
}