namespace ET.Server
{
    /// <summary>
    /// 给周围目标添加Buff
    /// BuffId;Buff时长;最大数量;万分比
    /// </summary>
    [Buff("MassAddBuff")]
    public class MassAddBuffEffect: ABuffEffect
    {
        protected override void OnTimeOut(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            int buffId = effectArgs.Args[0];
            int time = effectArgs.Args[1];
            int maxCount = effectArgs.Args[2];
            int pct = effectArgs.Args[3];
        }
    }
}