using System.Collections.Generic;

namespace ET.Client
{
    public enum MenuSelectMode
    {
        /// <summary>
        /// 不选择
        /// </summary>
        None,

        /// <summary>
        /// 选中第一个
        /// </summary>
        First,

        /// <summary>
        /// 自定义选中
        /// </summary>
        Custom,
    }

    [EnableMethod]
    public class MenuDict: Entity, IAwake<MenuSelectMode>
    {
        public Dictionary<int, Scroll_Item_Menu> MenuDic { get; } = new();

        /// <summary>
        /// 菜单选择模式
        /// </summary>
        public MenuSelectMode SelectMode { get; set; }

        /// <summary>
        /// 当前选择的菜单ID
        /// </summary>
        public int SelectId { get; set; }
    }
}