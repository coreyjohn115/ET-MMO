using System.Collections.Generic;

namespace ET
{
    public abstract class ActionSubConfig : Object
    {
    }

    public partial class ActionConfig
    {
        private ActionSubConfig subConfig;

        public T GetSubConfig<T>() where T : ActionSubConfig
        {
            if (this.subConfig == null)
            {
                this.subConfig = MongoHelper.FromJson<T>(this.JsonStr);
            }

            return this.subConfig as T;
        }
    }

    public partial class ActionConfigCategory
    {
        private Dictionary<string, ActionConfig> actionDict = new();

        public ActionConfig GetActionCfg(string actionName)
        {
            if (!this.actionDict.TryGetValue(actionName, out var value))
            {
                Thrower.Throw("ActionConfig not found");
            }

            return value;
        }

        public override void EndInit()
        {
            foreach (ActionConfig v in this.dict.Values)
            {
                this.actionDict.TryAdd(v.Name, v);
            }
        }
    }
}