using System.Collections.Generic;
using System.Linq;

namespace ET.Client
{
    [EntitySystemOf(typeof (ActionUnit))]
    public static partial class ActionUnitSystem
    {
        [EntitySystem]
        private static void Awake(this ActionUnit self, string name, float duration)
        {
            self.tcs = ETTask.Create(true);
            self.ActionName = name;
            List<ActionConfig> configs = ActionGroupConfigCategory.Instance.GetActionConfig(name);
            if (configs == default)
            {
                Log.Error($"获取行为配置失败: {name}");
                return;
            }

            foreach (ActionConfig config in configs)
            {
                self.AddChild<ActionSubUnit, string, float, ActionConfig>(name, duration, config);
            }
        }

        [EntitySystem]
        private static void Destroy(this ActionUnit self)
        {
            self.Finish();
        }

        public static void Finish(this ActionUnit self)
        {
            if (self.Children.Count <= 0)
            {
                return;
            }

            foreach (ActionSubUnit child in self.Children.Values.ToList())
            {
                child.Finish();
            }

            self.tcs.SetResult();
        }

        public static bool IsFinish(this ActionUnit self)
        {
            bool finish = true;
            foreach (ActionSubUnit children in self.Children.Values)
            {
                if (children.IsRunning)
                {
                    finish = false;
                }
            }

            return finish;
        }

        public static async ETTask WaitFinishAsync(this ActionUnit self)
        {
            await self.tcs;
        }

        public static void Update(this ActionUnit self)
        {
            foreach (ActionSubUnit child in self.Children.Values)
            {
                child.Update();
            }
        }
    }
}