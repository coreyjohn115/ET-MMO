namespace ET.Server
{
    public interface IBuffEffect
    {
        /// <summary>
        /// 天赋是否生效过
        /// </summary>
        bool IsBsEffect { get; set; }

        /// <summary>
        /// Buff 创建时
        /// </summary>
        void Create(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args);

        /// <summary>
        /// Buff 时间间隔到
        /// </summary>
        void Update(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args);

        /// <summary>
        /// 触发Buff事件时
        /// </summary>
        /// <param name="self"></param>
        /// <param name="buffEvent">Buff事件类型</param>
        /// <param name="buff"></param>
        /// <param name="effectArgs"></param>
        /// <param name="args">额外参数</param>
        void Event(BuffComponent self, BuffEvent buffEvent, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args);

        /// <summary>
        /// 计时时间到
        /// </summary>
        void TimeOut(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args);

        /// <summary>
        /// buff 移除时
        /// </summary>
        void Remove(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args);
    }
}