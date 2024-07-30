namespace ET.Server
{
    /// <summary>
    /// 添加能力码
    /// </summary>
    [Buff("AddAbility")]
    public class AddAbilityEffect: ABuffEffect
    {
        protected override void OnCreate(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            foreach (int ability in effectArgs.Args)
            {
                self.GetParent<Unit>().GetComponent<AbilityComponent>().AddAbility(ability);
            }
        }

        protected override void OnRemove(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            foreach (int ability in effectArgs.Args)
            {
                self.GetParent<Unit>().GetComponent<AbilityComponent>().RemoveAbility(ability);
            }
        }
    }
}