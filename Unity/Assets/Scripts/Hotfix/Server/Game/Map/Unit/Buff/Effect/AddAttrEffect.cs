namespace ET.Server
{
    /// <summary>
    /// 添加属性
    /// </summary>
    [Buff("AddAttr")]
    public class AddAttrEffect: ABuffEffect
    {
        protected override void OnCreate(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            var numericCom = self.GetParent<Unit>().GetComponent<NumericComponent>();
            for (int i = 0; i < effectArgs.Args.Count / 2; i++)
            {
                if (effectArgs.Args[i * 2] <= 0)
                {
                    continue;
                }

                var attrType = effectArgs.Args[i * 2];
                var attrValue = effectArgs.Args[i * 2 + 1];
                numericCom.Add(attrType, attrValue);
            }
        }

        protected override void OnRemove(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            var numericCom = self.GetParent<Unit>().GetComponent<NumericComponent>();
            for (int i = 0; i < effectArgs.Args.Count / 2; i++)
            {
                if (effectArgs.Args[i * 2] <= 0)
                {
                    continue;
                }

                var attrType = effectArgs.Args[i * 2];
                var attrValue = effectArgs.Args[i * 2 + 1];
                numericCom.Add(attrType, -attrValue);
            }
        }
    }
}