using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 动画序列
    /// </summary>
    public sealed class Sequence: Tweener
    {
        #region Methods

        public Sequence Append(Tweener tweener)
        {
            if (!this.Verify(tweener))
            {
                return this;
            }

            var list = new List<Tweener>();
            this.lists.Add(list);
            this.currentList = list;

            tweener.OnComplete += _ => { TweenerCompleted(list); };
            this.currentList.Add(tweener);
            this.CheckDuration();

            return this;
        }

        public Sequence Join(Tweener tweener)
        {
            if (this.currentList == null)
            {
                Log.Error("请在调用Append后调用John");
                return default;
            }

            var list = this.currentList;
            tweener.OnComplete += _ => { TweenerCompleted(list); };
            this.currentList.Add(tweener);
            this.CheckDuration();

            return this;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// 验证是否可以添加或移除
        /// </summary>
        /// <param name="tweener"></param>
        /// <returns></returns>
        private bool Verify(Tweener tweener)
        {
            if (tweener == null)
            {
                Log.Error("添加空对象");

                return false;
            }

            if (IsPlaying || tweener.IsPlaying)
            {
                Log.Warning("不允许运行时添加或移除");

                return false;
            }

            if (!this.kill)
            {
                return true;
            }

            Log.Error("已经销毁的动画对象");

            return false;
        }

        protected override void OnStarted()
        {
            base.OnStarted();

            this.curIndex = 0;
            if (this.IsForward)
            {
                var list = this.lists[this.curIndex];
                foreach (Tweener tweener in list)
                {
                    tweener.PlayForward();
                }
            }
            else
            {
                var list = this.lists[this.curIndex];
                list.Reverse();
                foreach (Tweener tweener in list)
                {
                    tweener.PlayReverse();
                }
            }
        }

        protected override void OnPaused()
        {
            base.OnPaused();

            foreach (var ll in this.lists)
            {
                foreach (var t in ll)
                {
                    t.IsPaused = this.IsPaused;
                }
            }
        }

        protected override void OnKilled()
        {
            base.OnKilled();

            foreach (var ll in this.lists)
            {
                foreach (var t in ll)
                {
                    t.Kill();
                }
            }

            this.curIndex = 0;
            this.currentList = default;
            this.lists.Clear();
        }

        protected override void DoReset()
        {
            foreach (var ll in this.lists)
            {
                try
                {
                    foreach (var t in ll)
                    {
                        t.OnReset?.Invoke(t);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        /// <summary>
        /// 当动画完成时
        /// </summary>
        private void TweenerCompleted(List<Tweener> ll)
        {
            foreach (Tweener t in ll)
            {
                if (!t.IsComplete)
                {
                    return;
                }
            }

            this.curIndex++;
            var list = this.lists.Get(this.curIndex);
            if (list != null)
            {
                if (this.IsForward)
                {
                    foreach (Tweener t in list)
                    {
                        t.PlayForward();
                    }
                }
                else
                {
                    list.Reverse();
                    foreach (Tweener t in list)
                    {
                        t.PlayReverse();
                    }
                }
            }
        }

        private void CheckDuration()
        {
            float max = 0;
            foreach (var list in this.lists)
            {
                float dur = 0;
                foreach (Tweener t in list)
                {
                    dur = Mathf.Max(dur, t.Delay + t.Duration);
                }

                max += dur;
            }

            this.Duration = max;
        }

        #endregion

        #region Internal Fields

        private List<Tweener> currentList;
        public int curIndex;
        private readonly List<List<Tweener>> lists = new List<List<Tweener>>();

        #endregion
    }
}