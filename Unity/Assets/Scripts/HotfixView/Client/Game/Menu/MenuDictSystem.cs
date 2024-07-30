using System;

namespace ET.Client
{
    [EntitySystemOf(typeof (MenuDict))]
    public static partial class MenuDictSystem
    {
        [EntitySystem]
        private static void Awake(this MenuDict self, MenuSelectMode mode)
        {
            self.SelectMode = mode;
            switch (mode)
            {
                case MenuSelectMode.None:
                    self.SelectId = -1;
                    break;
                case MenuSelectMode.First:
                    self.SelectId = 0;
                    break;
                case MenuSelectMode.Custom:
                    break;
            }
        }

        public static Scroll_Item_Menu GetCurrentMenu(this MenuDict self)
        {
            return self.MenuDic[self.SelectId];
        }

        public static int GetGroupId(this MenuDict self)
        {
            return self.GetCurrentMenu().MeunData.Config.GroupId;
        }
    }
}