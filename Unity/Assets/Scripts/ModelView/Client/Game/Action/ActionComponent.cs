using System.Collections.Generic;

namespace ET.Client
{
    public struct PlayActionEvent
    {
        public Unit Unit;
        public string ViewCmd;
    }

    public struct PushActionEvent
    {
        public Unit Unit;
        public string ViewCmd;
    }

    [ComponentOf(typeof (Unit))]
    public class ActionComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public List<long> pushActions = new List<long>();

        public EntityRef<ActionUnit> curAction;

        public List<long> playActions = new List<long>();
    }
}