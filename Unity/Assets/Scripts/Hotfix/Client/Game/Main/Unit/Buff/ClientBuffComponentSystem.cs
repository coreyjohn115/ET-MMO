using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof (ClientBuffComponent))]
    [FriendOf(typeof (ClientBuffComponent))]
    [FriendOf(typeof (ClientBuffUnit))]
    public static partial class ClientBuffComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientBuffComponent self)
        {
        }

        public static void UpdateBuff(this ClientBuffComponent self, M2C_UpdateBuff message)
        {
            var unit = self.GetChild<ClientBuffUnit>(message.Id);
            if (!unit)
            {
                unit = self.AddChildWithId<ClientBuffUnit>(message.Id);
                unit.cfgId = message.CfgId;
            }

            unit.Layer = message.Layer;
            unit.ValidTime = message.ValidTime;

            string viewCmd = unit.Config.ViewCmd;
            if (viewCmd.IsNullOrEmpty())
            {
                return;
            }

            if (!self.buffListDict.TryGetValue(viewCmd, out var list))
            {
                list = new HashSet<long>();
                self.buffListDict.Add(viewCmd, list);
                EventSystem.Instance.Publish(self.Scene(), new AddBuffView() { ViewCmd = viewCmd, Unit = self.GetParent<Unit>() });
            }

            list.Add(message.Id);
        }

        public static void DelBuff(this ClientBuffComponent self, long id)
        {
            var unit = self.GetChild<ClientBuffUnit>(id);
            if (!unit)
            {
                return;
            }

            string viewCmd = unit.Config.ViewCmd;
            if (viewCmd.IsNullOrEmpty())
            {
                return;
            }

            if (!self.buffListDict.TryGetValue(viewCmd, out var list))
            {
                return;
            }

            list.Remove(id);
            if (list.Count == 0)
            {
                self.buffListDict.Remove(viewCmd);
                EventSystem.Instance.Publish(self.Scene(), new RemoveBuffView() { ViewCmd = viewCmd, Unit = self.GetParent<Unit>() });
            }

            self.RemoveChild(id);
        }
    }
}