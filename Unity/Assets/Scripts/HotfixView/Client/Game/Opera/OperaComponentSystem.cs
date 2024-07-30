using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ET.Client
{
    [EntitySystemOf(typeof (OperaComponent))]
    [FriendOf(typeof (OperaComponent))]
    public static partial class OperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this OperaComponent self)
        {
            LeanTouch.OnFingerTap += self.OnFingerTap;
            LeanTouch.OnFingerDown += self.OnFingerDown;
        }

        [EntitySystem]
        private static void Destroy(this OperaComponent self)
        {
            LeanTouch.OnFingerTap -= self.OnFingerTap;
            LeanTouch.OnFingerDown -= self.OnFingerDown;
        }

        [EntitySystem]
        private static void Update(this OperaComponent self)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                CodeLoader.Instance.Reload();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIComponentHelper.CheckCloseUI(self).NoContext();
            }
        }

        private static void CheckClosePopUI(this OperaComponent self, Vector2 pos)
        {
            //检测pop_win
            var list = LeanTouch.RaycastGui(pos, -1);
            foreach (RaycastResult result in list)
            {
                if (result.gameObject.CompareTag("PopUI"))
                {
                    return;
                }
            }

            self.Root().GetComponent<UIComponent>().HidePopWindow();
        }

        private static void OnFingerDown(this OperaComponent self, LeanFinger finger)
        {
            self.CheckClosePopUI(finger.ScreenPosition);
        }

        private static void OnFingerTap(this OperaComponent self, LeanFinger finger)
        {
            if (finger.IsOverGui)
            {
                return;
            }

            Unit myUnit = UnitHelper.GetMyUnitFromClientScene(self.Scene());
            if (!myUnit || !myUnit.CanMove())
            {
                return;
            }

            Ray ray = Global.Instance.MainCamera.ScreenPointToRay(finger.ScreenPosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 1000f, LayerHelper.LayerMap))
            {
                return;
            }

            C2M_PathfindingResult c2MPathfindingResult = C2M_PathfindingResult.Create();
            c2MPathfindingResult.Position = hit.point;
            self.clickPoint = hit.point;
            self.Root().GetComponent<ClientSenderComponent>().Send(c2MPathfindingResult);
        }
    }
}