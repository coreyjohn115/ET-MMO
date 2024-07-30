using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 动画管理器
    /// </summary>
    [Code]
    public class TweenManager: Singleton<TweenManager>, ISingletonAwake, ISingletonUpdate, ISingletonLateUpdate
    {
        /// <summary>
        /// 创建Tweener
        /// </summary>
        /// <returns></returns>
        public T CreateTweener<T>() where T : Tweener
        {
            var t = ObjectPool.Instance.Fetch(typeof (T));
            var tweener = (T)t;
            tweener.Init();
            return tweener;
        }

        /// <summary>
        /// 销毁动画
        /// </summary>
        /// <param name="tweener">要销毁的动画</param>
        /// <param name="complete"></param>
        public void Kill(Tweener tweener, bool complete)
        {
            tweener.Kill(complete);
        }

        public void Awake()
        {
            fixedUpdatedList = new List<Tweener>();
            updatedList = new List<Tweener>();
        }

        /// <summary>
        /// 注册动画到动画更新字典中
        /// </summary>
        /// <param name="tweener">要注册的动画</param>
        internal void RegisterTweener(Tweener tweener)
        {
            Thrower.IsNotNull(tweener);
            if (tweener.UseFixedUpdate)
            {
                if (fixedUpdatedList.Contains(tweener))
                {
                    Log.Warning($"重复添加动画ID: {tweener.Id}");

                    return;
                }

                fixedUpdatedList.Add(tweener);
            }
            else
            {
                if (updatedList.Contains(tweener))
                {
                    Log.Warning($"重复添加动画ID: {tweener.Id}");

                    return;
                }

                updatedList.Add(tweener);
            }
        }

        /// <summary>
        /// 取消动画更新
        /// </summary>
        /// <param name="tweener">要取消更新的动画</param>
        internal bool UnRegisterTweener(Tweener tweener)
        {
            Thrower.IsNotNull(tweener);
            if (tweener.UseFixedUpdate)
            {
                return fixedUpdatedList.Remove(tweener);
            }

            return updatedList.Remove(tweener);
        }

        void ISingletonUpdate.Update()
        {
            var deltaTime = Time.deltaTime;
            var time = Time.time;
            for (int i = 0; i < updatedList.Count; i++)
            {
                var item = updatedList[i];
                try
                {
                    item.Update(deltaTime, time);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        void ISingletonLateUpdate.LateUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            var time = Time.time;
            for (int i = 0; i < fixedUpdatedList.Count; i++)
            {
                var item = fixedUpdatedList[i];
                try
                {
                    item.Update(deltaTime, time);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        protected override void Destroy()
        {
            for (int i = 0; i < updatedList.Count; i++)
            {
                var item = updatedList[i];
                item.Kill();
            }

            for (int i = 0; i < fixedUpdatedList.Count; i++)
            {
                var item = updatedList[i];
                item.Kill();
            }

            fixedUpdatedList.Clear();
            updatedList.Clear();
        }

        private List<Tweener> fixedUpdatedList;
        private List<Tweener> updatedList;
    }
}