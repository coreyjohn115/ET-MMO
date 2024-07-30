using System;

namespace ET.Server
{
    public abstract class ABuffEffect: IBuffEffect
    {
        public bool IsBsEffect { get; set; }

        public void Create(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            try
            {
                OnCreate(self, buff, dyna, effectArgs, args);
            }
            catch (Exception e)
            {
                Log.Error($"执行Buff创建事件错误: {e}");
            }
        }

        public void Update(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            try
            {
                OnUpdate(self, buff, dyna, effectArgs, args);
            }
            catch (Exception e)
            {
                Log.Error($"执行Buff创建事件错误: {e}");
            }
        }

        public void Event(BuffComponent self, BuffEvent buffEvent, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            try
            {
                OnEvent(self, buffEvent, buff, dyna, effectArgs, args);
            }
            catch (Exception e)
            {
                Log.Error($"执行Buff事件错误: {buffEvent} {e}");
            }
        }

        public void TimeOut(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            try
            {
                OnTimeOut(self, buff, dyna, effectArgs, args);
            }
            catch (Exception e)
            {
                Log.Error($"执行Buff超时事件错误: {e}");
            }
        }

        public void Remove(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
            try
            {
                OnRemove(self, buff, dyna, effectArgs, args);
            }
            catch (Exception e)
            {
                Log.Error($"执行Buff移除事件错误: {e}");
            }
        }

        protected virtual void OnCreate(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
        }

        protected virtual void OnUpdate(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
        }

        protected virtual void OnEvent(BuffComponent self, BuffEvent buffEvent, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
        }

        protected virtual void OnTimeOut(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
        }

        protected virtual void OnRemove(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
        {
        }
    }
}