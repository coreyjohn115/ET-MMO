using System.Collections.Generic;

namespace ET.Client
{
    public static class WindowItemRes
    {
        [StaticField]
        public static Dictionary<WindowID, List<string>> WindowItemResDictionary = new Dictionary<WindowID, List<string>>()
        {
			{ WindowID.Win_UIServerList, new List<string>() { "Item_Server", } },
			{ WindowID.Win_UIMain, new List<string>() { "Item_Main_Menu", } },
			{ WindowID.Win_UIBag, new List<string>() { "Item_Bag_Left_Menu", } },
			{ WindowID.Win_UITools, new List<string>() {} },
			{ WindowID.Win_UIGm, new List<string>() { "Item_Gm", } },
        };
    }
}
