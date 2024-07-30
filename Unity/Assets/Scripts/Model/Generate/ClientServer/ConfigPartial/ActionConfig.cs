using System.Collections.Generic;

namespace ET
{
    public class AActionSubConfig
    {
        public float StartTime;
        public float Duration;
        public int Priority;
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