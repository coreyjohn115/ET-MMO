using System;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 动画基础实现
    /// </summary>
    public class Tweener: IPool
    {
        #region Events

        /// <summary>
        /// 当动画开始
        /// </summary>
        public Action<Tweener> OnStart;

        /// <summary>
        /// 当动画暂停时
        /// </summary>
        public Action<Tweener> OnPause;

        /// <summary>
        /// 当动画更新时
        /// </summary>
        public Action<Tweener> OnUpdated;

        /// <summary>
        /// 当动画销毁时
        /// </summary>
        public Action<Tweener> OnKill;

        /// <summary>
        /// 当动画完成时
        /// </summary>
        public Action<Tweener> OnComplete;

        public Action<Tweener> OnReset;

        /// <summary>
        /// 当所有动画完成时
        /// </summary>
        public Action<Tweener> OnAllComplete;

        /// <summary>
        /// 更新采样因子
        /// </summary>
        public Action<float, float> UpdateFactor;

        #endregion

        #region Properties

        bool IPool.IsFromPool { get; set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; set; }

        /// <summary>
        /// 动画唯一ID
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// 动画持续时间
        /// </summary>
        public float Duration
        {
            get => duration;
            set
            {
                if (Math.Abs(this.duration - value) > 0.0001f)
                {
                    duration = value;
                    amountPerDelta = Math.Abs(1f / duration * Math.Sign(amountPerDelta));
                }
            }
        }

        public bool UseFixedUpdate { get; set; }

        /// <summary>
        /// 是否动画处于播放中
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// 是否动画处于暂停中
        /// </summary>
        public bool IsPaused
        {
            get => pause;
            set
            {
                if (pause != value)
                {
                    pause = value;
                    try
                    {
                        OnPause?.Invoke(this);
                        OnPaused();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
        }

        /// <summary>
        /// 缓动效果
        /// </summary>
        public Ease EaseType { get; set; } = Ease.Linear;

        /// <summary>
        /// </summary>
        public float EaseOvershootOrAmplitude => 1.70158f;

        /// <summary>
        /// </summary>
        public float EasePeriod { get; set; }

        /// <summary>
        /// 动态执行延迟
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        /// 动画循环类型
        /// </summary>
        public LoopType LoopType { get; set; } = LoopType.None;

        /// <summary>
        /// 动画播放次数 默认播放一次 小于等于0时 为无限循环
        /// </summary>
        public int RepeatTime { get; set; } = 1;

        /// <summary>
        /// 是否自动销毁
        /// </summary>
        public bool AutoKill { get; set; } = true;

        /// <summary>
        /// 动画是否处于完成状态
        /// </summary>
        public bool IsComplete { get; protected set; }

        /// <summary>
        /// 是否正方向播放动画
        /// </summary>
        public bool IsForward { get; private set; }

        #endregion

        #region Methods

        public Tweener()
        {
            Id = ++idGenerate;
        }

        /// <summary>
        /// 停止动画并重置除事件外所有数据
        /// </summary>
        public void Reset(bool clearEvent = false)
        {
            Stop();

            IsPaused = false;
            factor = 0;
            currentTime = 0;
            started = false;
            startTime = 0;
            curve = default;
            if (clearEvent)
            {
                ClearEvent();
            }

            try
            {
                OnReset?.Invoke(this);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            DoReset();
        }

        /// <summary>
        /// 重新开始动画
        /// </summary>
        public void ReStart()
        {
            Reset();
            this.PlayForward();
        }

        /// <summary>
        /// 播放当前动画
        /// </summary>
        public void PlayForward()
        {
            this.IsForward = true;
            Play(true);
        }

        /// <summary>
        /// 反方播放动画
        /// </summary>
        public void PlayReverse()
        {
            this.IsForward = false;
            Play(false);
        }

        /// <summary>
        /// 停止播放动画
        /// </summary>
        /// <param name="forceToEnd">是否强制完成</param>
        /// <param name="complete">  </param>
        public void Stop(bool forceToEnd = false, bool complete = true)
        {
            if (!IsPlaying || kill)
            {
                return;
            }

            IsPlaying = false;
            TweenManager.Instance.UnRegisterTweener(this);
            if (forceToEnd)
            {
                Sample(1, true);
            }

            if (complete)
            {
                Complete();
            }
        }

        /// <summary>
        /// 销毁当前动画
        /// </summary>
        public void Kill(bool complete = true)
        {
            if (kill)
            {
                return;
            }

            try
            {
                Stop(false, complete);
                OnKill?.Invoke(this);
                OnKilled();
                UserData = null;
                this.ClearEvent();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            kill = true;
        }

        /// <summary>
        /// 等待动画完成
        /// </summary>
        public async ETTask AwaitForComplete()
        {
            this.ecs = ETTask.Create();
            await this.ecs;
        }

        /// <summary>
        /// 设置动画曲线
        /// </summary>
        /// <param name="c"></param>
        public Tweener SetCurve(AnimationCurve c)
        {
            this.curve = c;
            return this;
        }

        #endregion

        #region Internal Methods

        internal void Init()
        {
            currentTime = 0;
            kill = false;
            started = false;
            startTime = 0;
            factor = 0;
            Duration = 1;
            UserData = null;
            IsPlaying = false;
            IsPaused = false;
            EaseType = Ease.Linear;
            Delay = 0;
            LoopType = LoopType.None;
            this.ClearEvent();
        }

        /// <summary>
        /// 当更新时
        /// </summary>
        /// <param name="factor">     采样因子 大小在0 - 1之间</param>
        /// <param name="currentTime"></param>
        protected virtual void OnUpdate(float factor, float currentTime)
        {
            try
            {
                UpdateFactor?.Invoke(factor, currentTime);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        protected virtual void DoReset()
        {
        }

        protected virtual void OnStarted()
        {
        }

        protected virtual void OnPaused()
        {
        }

        protected virtual void OnKilled()
        {
            if (ecs != null)
            {
                ecs.SetResult();
                ecs = null;
            }
        }

        /// <summary>
        /// 当完成一个动画的回调
        /// </summary>
        protected virtual void OnCompleted()
        {
            if (ecs != null)
            {
                ecs.SetResult();
                ecs = null;
            }
        }

        private void Complete()
        {
            IsComplete = true;
            try
            {
                OnComplete?.Invoke(this);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            if (RepeatTime > 0)
            {
                currentTime++;
                if (currentTime == RepeatTime)
                {
                    currentTime = 0;
                    try
                    {
                        OnAllComplete?.Invoke(this);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }

                    if (AutoKill)
                    {
                        Kill();
                    }
                }
            }

            OnCompleted();
        }

        private void UpdatePerDelta(bool forward)
        {
            amountPerDelta = Mathf.Abs(amountPerDelta);
            if (!forward)
            {
                amountPerDelta = -amountPerDelta;
            }
        }

        private void Play(bool forward)
        {
            if (kill)
            {
                Log.Error("无法播放已经销毁的动画");

                return;
            }

            this.IsPaused = false;
            if (IsPlaying)
            {
                if (this.IsForward != forward)
                {
                    this.IsForward = forward;
                    UpdatePerDelta(forward);
                }

                return;
            }

            UpdatePerDelta(forward);
            IsPlaying = true;
            TweenManager.Instance.RegisterTweener(this);
            try
            {
                OnStart?.Invoke(this);
                OnStarted();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        internal void Update(float delta, float time)
        {
            if (IsPaused)
            {
                return;
            }

            //真正开始动画需要等待动画延时
            if (!started)
            {
                delta = 0;
                started = true;
                startTime = time + Delay;
            }

            if (time <= startTime)
            {
                return;
            }

            if (RepeatTime > 0 && currentTime == RepeatTime)
            {
                Finish();
                return;
            }

            //提前采样因子
            factor += Duration == 0f? 1f : amountPerDelta * delta;
            if (LoopType == LoopType.Restart)
            {
                //循环模式，当因子超过1之后重置它
                if (factor > 1f)
                {
                    factor -= (float)Math.Floor(factor);
                }
            }
            else if (LoopType == LoopType.PingPong)
            {
                // Ping-pong效果反转方向
                if (factor > 1f)
                {
                    factor = 1f - (factor - (float)Math.Floor(factor));
                    amountPerDelta = -amountPerDelta;
                }
                else if (factor < 0f)
                {
                    factor = -factor;
                    factor -= (float)Math.Floor(factor);
                    amountPerDelta = -amountPerDelta;
                }
            }
            else
            {
                if (factor > 1f || factor < 0)
                {
                    Finish();
                    return;
                }
            }

            //因子超出范围，最最后的调整
            if (RepeatTime > 0 && currentTime == RepeatTime && (Duration == 0f || factor > 1f || factor < 0f))
            {
                Finish();
            }
            else
            {
                Sample(factor, false);
            }

            return;

            void Finish()
            {
                this.factor = this.IsForward? 1f : 0f;
                this.Sample(this.factor, true);
                this.IsPlaying = false;
                TweenManager.Instance.UnRegisterTweener(this);
                this.Complete();
            }
        }

        /// <summary>
        /// 采样
        /// </summary>
        /// <param name="factor">    采样因子</param>
        /// <param name="isFinished">是否完成</param>
        private void Sample(float factor, bool isFinished)
        {
            if (isFinished)
            {
                OnUpdate(factor, Duration);

                return;
            }

            var newFactor = Math.Max(Math.Min(factor, 1), 0);
            float time = newFactor * Duration;
            float value = 0;
            if (this.curve != null)
            {
                value = this.curve.Evaluate(time);
            }
            else
            {
                value = EaseManager.Evaluate(EaseType, time, Duration,
                    EaseOvershootOrAmplitude, EasePeriod);
            }

            OnUpdate(value, time);
            try
            {
                OnUpdated?.Invoke(this);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ClearEvent()
        {
            OnStart = null;
            OnPause = null;
            OnUpdated = null;
            OnComplete = null;
            OnKill = null;
            OnAllComplete = null;
            UpdateFactor = null;
            OnReset = null;
        }

        #endregion

        #region Internal Fields

        protected bool kill;

        private AnimationCurve curve;
        private ETTask ecs;
        private int idGenerate;
        private int currentTime;
        private bool pause;
        private bool started;
        private float startTime;
        private float duration;
        private float amountPerDelta = 1000f;
        private float factor;

        #endregion
    }
}