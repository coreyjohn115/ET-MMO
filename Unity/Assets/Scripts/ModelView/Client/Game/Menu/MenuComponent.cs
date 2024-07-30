using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 菜单选择事件
    /// </summary>
    public struct MenuSelectEvent
    {
        public int Index;

        public MeunData Data;

        public Entity ItemMenu;
    }

    /// <summary>
    /// 菜单管理
    /// </summary>
    [ComponentOf(typeof (Scene))]
    public class MenuComponent: Entity, IAwake
    {
        public Dictionary<int, List<MeunData>> menuDict = new();
    }
}