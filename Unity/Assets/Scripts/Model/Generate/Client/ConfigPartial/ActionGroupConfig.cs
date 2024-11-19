using System.Collections.Generic;

namespace ET
{
    public partial class ActionGroupConfigCategory
    {
        private readonly Dictionary<string, List<ActionConfig>> nameConfigDict = new();

        public override void EndInit()
        {
            base.EndInit();

            foreach (ActionGroupConfig config in this.dict.Values)
            {
                this.nameConfigDict.Add(config.Name, config.ActionList);
            }
        }

        /// <summary>
        /// 通过名称获取行为组配置
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public List<ActionConfig> GetActionConfig(string actionName)
        {
            return this.nameConfigDict.GetValueOrDefault(actionName);
        }
    }
}