using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (ES_EquipItem))]
    [FriendOf(typeof (ES_EquipItem))]
    public static partial class ES_EquipItemSystem
    {
        [UICom(nameof (ES_EquipItem))]
        private class ES_NormalItemShow: AUIComHandler
        {
            public override void Show(Entity uiCom)
            {
                uiCom.GetParent<ES_EquipItem>().Show().NoContext();
            }
        }

        [EntitySystem]
        private static void Awake(this ES_EquipItem self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_EquipItem self)
        {
            self.DestroyWidget();
        }

        private static async ETTask Show(this ES_EquipItem self)
        {
            ItemData item = self.itemData;
            self.E_DescExtendText.SetText(item.Config.Desc);
            LayoutRebuilder.ForceRebuildLayoutImmediate(self.uiTransform as RectTransform);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(self.uiTransform.parent as RectTransform, Input.mousePosition,
                Global.Instance.UICamera, out Vector3 pos);
            UIHelper.RelativeAnchorItem(self.uiTransform as RectTransform, pos, AnchorLimitType.Left, 50f);

            self.ESItem.SetItemData(self.itemData);
            await self.ESItem.RefreshFrame();
            await self.ESItem.RefreShIcon();
        }
    }
}