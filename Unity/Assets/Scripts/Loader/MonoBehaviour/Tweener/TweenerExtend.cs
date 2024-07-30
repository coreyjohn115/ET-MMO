using UnityEngine;

namespace ET.Client
{
    public static class TweenerExtend
    {
        public static Tweener SetAutoSkill(this Tweener self, bool kill)
        {
            self.AutoKill = kill;
            return self;
        }

        public static Tweener SetDelay(this Tweener self, float delay)
        {
            self.Delay = delay;
            return self;
        }

        public static Tweener SetDuration(this Tweener self, float duration)
        {
            self.Duration = duration;
            return self;
        }

        public static Tweener SetEase(this Tweener self, Ease ease)
        {
            self.EaseType = ease;
            return self;
        }

        public static Tweener Pause(this Tweener self)
        {
            self.IsPaused = true;
            return self;
        }
        
        public static Tweener SetLoopType(this Tweener self, LoopType t)
        {
            self.LoopType = t;
            return self;
        }

        public static Tweener DOScale(this Transform self, Vector3 endValue, float time, Vector3? start = default)
        {
            var t = TweenManager.Instance.CreateTweener<Tweener>();
            t.AutoKill = false;
            t.Duration = time;
            Vector3 startV = self.localScale;
            if (start != null)
            {
                startV = start.Value;
            }

            t.UpdateFactor += (factor, _) => { self.localScale = Vector3.Lerp(startV, endValue, factor); };
            t.OnReset += _ => { self.localScale = startV; };
            return t;
        }

        public static Tweener DOScale(this Transform self, float endValue, float time, float? start = default)
        {
            var t = TweenManager.Instance.CreateTweener<Tweener>();
            t.AutoKill = false;
            t.Duration = time;
            Vector3 startV = self.localScale;
            if (start != null)
            {
                startV = start.Value * Vector3.one;
            }

            Vector3 endV = Vector3.one * endValue;
            t.UpdateFactor += (factor, _) => { self.localScale = Vector3.Lerp(startV, endV, factor); };
            t.OnReset += _ => { self.localScale = startV; };
            return t;
        }

        public static Tweener DOAnchoredPosition3D(this RectTransform self, Vector3 endValue, float time, Vector3? start = default)
        {
            var t = TweenManager.Instance.CreateTweener<Tweener>();
            t.AutoKill = false;
            t.Duration = time;
            Vector3 startV = self.anchoredPosition3D;
            if (start != null)
            {
                startV = start.Value;
            }

            t.UpdateFactor = (factor, _) => { self.anchoredPosition3D = Vector3.Lerp(startV, endValue, factor); };
            t.OnReset += _ => { self.anchoredPosition3D = startV; };
            return t;
        }

        public static Tweener DOAnchoredPosition(this RectTransform self, Vector2 endValue, float time, Vector2? start = default)
        {
            var t = TweenManager.Instance.CreateTweener<Tweener>();
            t.AutoKill = false;
            t.Duration = time;
            Vector2 startV = start ?? self.anchoredPosition;
            t.UpdateFactor = (factor, _) => { self.anchoredPosition = Vector2.Lerp(startV, endValue, factor); };
            t.OnReset += _ => { self.anchoredPosition = startV; };
            return t;
        }

        public static Tweener DOAnchoredPositionX(this RectTransform self, float endValue, float time, float? start = default)
        {
            var t = TweenManager.Instance.CreateTweener<Tweener>();
            t.AutoKill = false;
            t.Duration = time;
            float startV = start ?? self.anchoredPosition.x;
            t.UpdateFactor = (factor, _) => { self.anchoredPosition = new Vector2(Mathf.Lerp(startV, endValue, factor), self.anchoredPosition.y); };
            t.OnReset += _ => { self.anchoredPosition = new Vector2(startV, self.anchoredPosition.y); };
            return t;
        }

        public static Tweener DOAnchoredPositionY(this RectTransform self, float endValue, float time, float? start = default)
        {
            var t = TweenManager.Instance.CreateTweener<Tweener>();
            t.AutoKill = false;
            t.Duration = time;
            float startV = start ?? self.anchoredPosition.y;
            t.UpdateFactor = (factor, _) => { self.anchoredPosition = new Vector2(self.anchoredPosition.x, Mathf.Lerp(startV, endValue, factor)); };
            t.OnReset += _ => { self.anchoredPosition = new Vector2(self.anchoredPosition.x, startV); };
            return t;
        }

        public static Tweener DOFade(this CanvasGroup self, float endValue, float time, float? start = default)
        {
            var t = TweenManager.Instance.CreateTweener<Tweener>();
            t.AutoKill = false;
            t.Duration = time;
            float startV = start ?? self.alpha;
            t.UpdateFactor += (factor, _) => { self.alpha = Mathf.Lerp(startV, endValue, factor); };
            t.OnReset += _ => { self.alpha = startV; };
            return t;
        }
    }
}