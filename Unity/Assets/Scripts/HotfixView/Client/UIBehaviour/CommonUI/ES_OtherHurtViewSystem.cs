using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (ES_OtherHurt))]
    [FriendOf(typeof (ES_OtherHurt))]
    public static partial class ES_OtherHurtSystem
    {
        [EntitySystem]
        private static void Awake(this ES_OtherHurt self, Transform transform)
        {
            self.uiTransform = transform;
            self.collector = transform.GetComponent<ReferenceCollector>();
        }

        [EntitySystem]
        private static void Destroy(this ES_OtherHurt self)
        {
            self.hurtInfo = default;
            self.sequenceLeft?.Kill();
            self.sequenceRight?.Kill();
            GameObjectPoolHelper.ReturnTransformToPool(self.uiTransform);
            self.DestroyWidget();
        }

        public static void Initliaze(this ES_OtherHurt self, long caster, HurtProto proto)
        {
            var view = self.Root().GetComponent<UIComponent>().GetDlgLogic<UIHud>().View;
            if (UnitHelper.IsMainUnit(self, caster))
            {
                self.uiTransform.SetParent(view.EG_SelfRectTransform, false);
            }
            else
            {
                self.uiTransform.SetParent(view.EG_OtherRectTransform, false);
            }

            self.caster = caster;
            self.hurtInfo = proto;
            self.sequenceLeft = TweenManager.Instance.CreateTweener<Sequence>();
            self.sequenceRight = TweenManager.Instance.CreateTweener<Sequence>();

            Tweener moveXL = self.E_TextText.rectTransform.DOAnchoredPositionX(-100, 0.3f, 0).SetEase(Ease.Linear);
            Tweener moveYL = self.E_TextText.rectTransform.DOAnchoredPositionY(100, 0.3f, 0).SetEase(Ease.Linear);
            Tweener scaleL = self.E_TextText.rectTransform.DOScale(1, 0.3f, 0.1f).SetEase(Ease.OutQuad);
            Tweener moveXL2 = self.E_TextText.rectTransform.DOAnchoredPositionX(-150, 0.6f, -100).SetEase(Ease.InCubic);
            Tweener moveYL2 = self.E_TextText.rectTransform.DOAnchoredPositionY(50, 0.6f, 100).SetEase(Ease.InCubic);
            Tweener alphaL = self.collector.Get<CanvasGroup>("CanvasGroup").DOFade(0, 0.6f).SetEase(Ease.InQuad);
            self.sequenceLeft.Append(moveXL);
            self.sequenceLeft.Join(moveYL);
            self.sequenceLeft.Join(scaleL);
            self.sequenceLeft.Append(moveXL2);
            self.sequenceLeft.Join(moveYL2);
            self.sequenceLeft.Join(alphaL);
            self.sequenceLeft.SetAutoSkill(false);
            self.sequenceLeft.OnStart += self.OnPlay;
            self.sequenceLeft.OnUpdated += self.OnUpdate;
            self.sequenceLeft.OnComplete += self.OnComplete;

            Tweener moveXR = self.E_TextText.rectTransform.DOAnchoredPositionX(100, 0.3f, 0).SetEase(Ease.Linear);
            Tweener moveYR = self.E_TextText.rectTransform.DOAnchoredPositionY(100, 0.3f, 0).SetEase(Ease.Linear);
            Tweener scaleR = self.E_TextText.rectTransform.DOScale(1, 0.3f, 0.1f).SetEase(Ease.OutQuad);
            Tweener moveXR2 = self.E_TextText.rectTransform.DOAnchoredPositionX(150, 0.6f, 100).SetEase(Ease.InCubic);
            Tweener moveYR2 = self.E_TextText.rectTransform.DOAnchoredPositionY(50, 0.6f, 100).SetEase(Ease.InCubic);
            Tweener alphaR = self.collector.Get<CanvasGroup>("CanvasGroup").DOFade(0, 0.6f).SetEase(Ease.InQuad);
            self.sequenceRight.Append(moveXR);
            self.sequenceRight.Join(moveYR);
            self.sequenceRight.Join(scaleR);
            self.sequenceRight.Append(moveXR2);
            self.sequenceRight.Join(moveYR2);
            self.sequenceRight.Join(alphaR);
            self.sequenceRight.SetAutoSkill(false);
            self.sequenceRight.OnStart += self.OnPlay;
            self.sequenceRight.OnUpdated += self.OnUpdate;
            self.sequenceRight.OnComplete += self.OnComplete;
        }

        private static void OnPlay(this ES_OtherHurt self, Tweener tweener)
        {
            Vector2 pos = Vector2.one;
            UIHelper.WorldToUI(self.startWorldPos, ref pos);
            (self.uiTransform as RectTransform).anchoredPosition = pos;
            self.E_ParryExtendImage.SetActive(self.hurtInfo.IsDirect);
            self.E_ReboundExtendImage.SetActive(self.hurtInfo.IsFender);
            self.E_TextText.text = self.hurtInfo.Hurt.ToString();
        }

        private static void OnUpdate(this ES_OtherHurt self, Tweener tweener)
        {
            Vector2 pos = Vector2.one;
            UIHelper.WorldToUI(self.startWorldPos, ref pos);
            (self.uiTransform as RectTransform).anchoredPosition = pos;
        }

        private static void OnComplete(this ES_OtherHurt self, Tweener tweener)
        {
            tweener?.Reset();
            self.Dispose();
        }

        public static void Play(this ES_OtherHurt self)
        {
            Unit unit = UnitHelper.GetUnitFromCurrentScene(self.Scene(), self.hurtInfo.Id);
            if (!unit)
            {
                self.OnComplete(default);
                return;
            }

            self.startWorldPos = unit.GetComponent<UnitGoComponent>().GetBone(UnitBone.Body).position;
            if (UIHelper.IsCastRight(self.Scene(), self.hurtInfo.Id, self.caster))
            {
                self.sequenceLeft.PlayForward();
            }
            else
            {
                self.sequenceRight.PlayForward();
            }
        }
    }
}