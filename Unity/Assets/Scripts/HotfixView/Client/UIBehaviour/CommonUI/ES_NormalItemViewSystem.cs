using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (ES_NormalItem))]
    [FriendOf(typeof (ES_NormalItem))]
    public static partial class ES_NormalItemSystem
    {
        [UICom(nameof (ES_NormalItem))]
        private class ES_NormalItemShow: AUIComHandler
        {
            public override void Show(Entity uiCom)
            {
                uiCom.GetParent<ES_NormalItem>().Show().NoContext();
            }
        }

        [EntitySystem]
        private static void Awake(this ES_NormalItem self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_NormalItem self)
        {
            self.DestroyWidget();
        }

        private static async ETTask Show(this ES_NormalItem self)
        {
            ItemData item = self.itemData;
            self.E_DescExtendText.SetText(item.Config.Desc);
            LayoutRebuilder.ForceRebuildLayoutImmediate(self.uiTransform as RectTransform);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(self.uiTransform.parent as RectTransform, Input.mousePosition,
                Global.Instance.UICamera, out Vector3 pos);
            UIHelper.RelativeAnchorItem(self.uiTransform as RectTransform, pos, AnchorLimitType.Left, 50f);

            QualityConfig qualityCfg = QualityConfigCategory.Instance.Get(item.Config.Quality);
            self.E_NameExtendText.SetText(item.Config.Name, qualityCfg.ColorBytes.BytesColor());
            self.E_BindExtendText.SetBindText(item.Bind);

            self.ESItem.SetItemData(self.itemData);
            await self.ESItem.RefreshFrame();
            await self.ESItem.RefreshIcon();
        }
    }
}