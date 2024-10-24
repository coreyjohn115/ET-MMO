using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Client
{
    [EntitySystemOf(typeof (MenuComponent))]
    [FriendOf(typeof (MenuComponent))]
    public static partial class MenuComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MenuComponent self)
        {
            foreach (var v in SystemMenuCategory.Instance.ClassifyDict)
            {
                self.InitMenu(v.Key, v.Value);
            }

            foreach (var v in EmojiConfigCategory.Instance.GetAll().Values.GroupBy(v => v.GroupId))
            {
                EmojiConfig cfg = v.FirstOrDefault();
                self.AddDynamicMenu(SystemMenuType.ChatEmojiMenu, new SystemMenu()
                {
                    Icon = cfg.Icon, Sort = cfg.Weight, Name = cfg.Name, GroupId = v.Key,
                });
            }
        }

        private static void InitMenu(this MenuComponent self, int classify, List<SystemMenu> list)
        {
            var ll = new List<MenuData>();
            foreach (SystemMenu cfg in list)
            {
                var menu = self.AddChild<MenuData, int>(cfg.Id);
                ll.Add(menu);
            }

            ll.Sort((a, b) => a.Config.Sort.CompareTo(b.Config.Sort));
            self.menuDict.Add(classify, ll);
        }

        public static List<MenuData> GetMenuList(this MenuComponent self, int type)
        {
            if (self.menuDict.TryGetValue(type, out var list))
            {
                return list;
            }

            return new List<MenuData>();
        }

        public static void AddDynamicMenu(this MenuComponent self, int type, SystemMenu config)
        {
            config.Classify = type;
            config.Id = RandomGenerator.RandInt32();
            SystemMenuCategory.Instance.AddDynamicMenu(config);
            if (!self.menuDict.TryGetValue(type, out var list))
            {
                list = new List<MenuData>();
                self.menuDict.Add(type, list);
            }

            var menu = self.AddChild<MenuData, int>(config.Id);
            list.Add(menu);
            list.Sort((a, b) => a.Config.Sort.CompareTo(b.Config.Sort));
        }

        public static void RemoveDynamicClassify(this MenuComponent self, int classify)
        {
            SystemMenuCategory.Instance.RemoveDynamicClassify(classify);
            if (!self.menuDict.Remove(classify, out var list))
            {
                return;
            }

            foreach (MenuData v in list)
            {
                self.RemoveChild(v.Id);
            }
        }

        public static void RemoveDynamicMenu(this MenuComponent self, int id)
        {
            var cfg = SystemMenuCategory.Instance.Get(id);
            if (cfg == null)
            {
                return;
            }

            self.RemoveChild(id);
            if (!self.menuDict.TryGetValue(cfg.Classify, out var list))
            {
                return;
            }

            MenuData d = list.FirstOrDefault(data => data.Id == id);
            if (d != null)
            {
                list.Remove(d);
            }

            SystemMenuCategory.Instance.RemoveDynamicMenu(id);
        }
    }
}