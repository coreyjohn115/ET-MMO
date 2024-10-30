using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace ET.Client
{
    public class SDK: Singleton<SDK>, ISingletonAwake
    {
        public void Awake()
        {
#if UNITY_WEBGL
            WXInit(OnCallback, "");
#endif
        }

#if UNITY_WEBGL
        [MonoPInvokeCallback(typeof (Action<string, string>))]
        private void OnCallback(string method, string result)
        {
            // 处理 JavaScript 的回调结果
            if (this.taskDict.TryGetValue(method, out var task))
            {
                task.SetResult($"{method}@@{result}");
            }
        }
#endif

        /// <summary>
        /// 调用SDK方法
        /// </summary>
        /// <param name="fun">sdk方法名</param>
        /// <param name="param">sdk参数</param>
        /// <returns></returns>
        public async ETTask<string> Call(string fun, string param)
        {
            await ETTask.CompletedTask;
            try
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return string.Empty;
#elif UNITY_ANDROID
                if (currentActivity == null)
                {
                    var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    currentActivity = activity.GetStatic<AndroidJavaObject>("currentActivity");
                }

                return currentActivity?.Call<string>("OnUnityMessage", fun, param);
#elif UNITY_IOS || UNITY_IPHONE
                return OnUnityMessage(fun, param);
#elif UNITY_WEBGL
                if (this.taskDict.ContainsKey(fun))
                {
                    Log.Error($"sdk上一次的请求还未完成! {fun}");
                    return string.Empty;
                }

                ETTask<string> t = ETTask<string>.Create();
                this.taskDict.Add(fun, t);
                WXInvoke(fun, param);
                return await t;
#else
                return string.Empty;
#endif
            }
            catch (Exception e)
            {
                Log.Error(e);
                return string.Empty;
            }
        }

        protected override void Destroy()
        {
            this.taskDict.Clear();
        }

#if UNITY_IOS || UNITY_IPHONE
        /// <summary>
        /// 获取SDK信息 同步返回
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="content">请求参数</param>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern string OnUnityMessage(string methodName, string content);
#elif UNITY_ANDROID
        private AndroidJavaObject currentActivity;
#elif UNITY_WEBGL
        //方法名与参数返回值要与JsLib里的方法名一模一样
        [DllImport("__Internal")]
        public static extern void WXInvoke(string method, string jsonParams);

        [DllImport("__Internal")]
        public static extern void WXInit(Action<string, string> callback, string jsonParams);
#endif

        private readonly Dictionary<string, ETTask<string>> taskDict = new();
    }
}