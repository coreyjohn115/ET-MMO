using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 菜单选择事件
    /// </summary>
    public struct MenuSelectEvent
    {
        public int Index;

        public MenuData Data;

        public Entity ItemMenu;
    }

    /// <summary>
    /// 菜单刷新事件
    /// </summary>
    public struct MenuRefreshEvent
    {
        public int MenuType;
    }

    /// <summary>
    /// 菜单管理
    /// </summary>
    [ComponentOf(typeof (Scene))]
    public class MenuComponent: Entity, IAwake
    {
        public Dictionary<int, List<MenuData>> menuDict = new();
    }
}