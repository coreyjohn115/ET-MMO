namespace ET.Server
{
    /// <summary>
    /// 免疫
    /// </summary>
    [Buff(EffectName)]
    public class ImmUnityEffect: ABuffEffect
    {
        public const string EffectName = "ImmUnity";
        
        protected override void OnCreate(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            self.GetParent<Unit>().GetComponent<AbilityComponent>().AddAbility((int)RoleAbility.ImmUnity);
        }

        protected override void OnRemove(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            self.GetParent<Unit>().GetComponent<AbilityComponent>().RemoveAbility((int)RoleAbility.ImmUnity);
        }
    }
}