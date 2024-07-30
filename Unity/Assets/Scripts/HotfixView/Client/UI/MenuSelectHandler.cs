using System;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class MenuSelectHandler: AEvent<Scene, MenuSelectEvent>
    {
        private static void SetActive(Entity entity, bool active)
        {
            foreach (Entity child in entity.Children.Values)
            {
                if (child is IUICom com)
                {
                    com.SetActive(active);
                }
            }
        }

        private static IUICom GetChild(Entity entity, Type t)
        {
            foreach (Entity child in entity.Children.Values)
            {
                if (child.GetType() == t)
                {
                    return child as IUICom;
                }
            }

            return default;
        }

        [EnableAccessEntiyChild]
        private static async ETTask OnMenuClick(Scene scene, MenuSelectEvent a, Entity entity, Transform parent)
        {
            Type t = CodeTypes.Instance.GetType($"ET.Client.{a.Data.Config.ComPath.Split('/')[1]}");
            SetActive(entity, false);
            IUICom child = GetChild(entity, t);
            if (child != default)
            {
                child.SetActive(true);
            }
            else
            {
                Transform trans = await UIHelper.LoadComUI(scene, a, parent);
                if (!trans)
                {
                    return;
                }

                Entity c = entity.AddChildByType(trans, t);
                c.AddComponent<UIComComponent>();
            }

            UIComComponent uiCom = entity.GetChildByName(t.Name).GetComponent<UIComComponent>();
            uiCom.Show();
        }

        protected override async ETTask Run(Scene scene, MenuSelectEvent a)
        {
            switch (a.Data.Config.Classify)
            {
                case SystemMenuType.Bag:
                    UIBag uiBag = a.ItemMenu.GetParent<UIBag>();
                    Transform parent = uiBag.View.UiTransform;
                    await OnMenuClick(scene, a, uiBag, parent);
                    break;
            }
        }
    }
}