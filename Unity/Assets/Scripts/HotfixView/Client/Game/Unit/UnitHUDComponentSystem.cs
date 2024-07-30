using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class BasicChange_RefreshHud: AEvent<Scene, BasicChangeEvent>
    {
        protected override async ETTask Run(Scene scene, BasicChangeEvent a)
        {
            //发布事件时 组件有可能还没创建
            a.Unit.GetComponent<UnitHUDComponent>()?.RefreshName();
            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Current)]
    public class UpdateShield_RefreshHud: AEvent<Scene, UpdateShield>
    {
        protected override async ETTask Run(Scene scene, UpdateShield a)
        {
            //发布事件时 组件有可能还没创建
            a.Unit.GetComponent<UnitHUDComponent>()?.RefreshHpBar();
            await ETTask.CompletedTask;
        }
    }

    /// <summary>
    /// 客户端监视hp数值变化，改变血条值
    /// </summary>
    [NumericWatcher(SceneType.Current, NumericType.Hp)]
    public class NumericWatcher_Hp_RefreshHud: INumericWatcher
    {
        public void Run(Unit unit, NumericChange args)
        {
            //发布事件时 组件有可能还没创建
            unit.GetComponent<UnitHUDComponent>()?.RefreshHpBar();
        }
    }

    [EntitySystemOf(typeof (UnitHUDComponent))]
    [FriendOf(typeof (UnitHUDComponent))]
    public static partial class UnitHUDComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this UnitHUDComponent self)
        {
            self.target = default;
            UnityEngine.Object.Destroy(self.root);
        }

        [EntitySystem]
        private static void Awake(this UnitHUDComponent self, GameObject go)
        {
            self.root = go;
            self.collector = go.GetComponent<ReferenceCollector>();
            self.gameObject = go.Get<GameObject>("Content");
            self.transform = self.gameObject.transform as RectTransform;
        }

        [EntitySystem]
        private static void LateUpdate(this UnitHUDComponent self)
        {
            if (!self.target)
            {
                return;
            }

            bool isVisible = UIHelper.WorldToUI(self.target.position, ref self.uiPos);
            if (!isVisible || !self.target.gameObject.activeSelf)
            {
                self.gameObject.SetActive(false);
            }
            else
            {
                self.gameObject.SetActive(true);
                self.transform.anchoredPosition = self.uiPos;
            }
        }

        public static void SetTarget(this UnitHUDComponent self, long id, Transform target)
        {
            self.target = target;
            self.unitId = id;
            self.RefreshName();
        }

        public static void RefreshName(this UnitHUDComponent self)
        {
            var unit = UnitHelper.GetUnitFromCurrentScene(self.Scene(), self.unitId);
            if (!unit)
            {
                return;
            }

            self.Get<ExtendText>("Name").text = unit.GetComponent<UnitBasic>().PlayerName;
            self.RefreshHpBar();
        }

        /// <summary>
        /// 刷新血条显示
        /// </summary>
        /// <param name="self"></param>
        public static void RefreshHpBar(this UnitHUDComponent self)
        {
            var unit = UnitHelper.GetUnitFromCurrentScene(self.Scene(), self.unitId);
            if (!unit)
            {
                return;
            }

            long hp = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.Hp);
            long maxHp = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.MaxHp);
            long shield = unit.GetComponent<ClientShieldComponent>().GetShield();
            self.Get<Slider>("HpArea").value = (float)hp / maxHp;
            self.Get<Slider>("ShieldArea").value = ((float)shield + hp) / maxHp;
        }

        private static T Get<T>(this UnitHUDComponent self, string name) where T : UnityEngine.Object
        {
            return self.collector.Get<T>(name);
        }
    }
}