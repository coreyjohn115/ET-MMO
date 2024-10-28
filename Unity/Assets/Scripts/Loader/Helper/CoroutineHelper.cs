using UnityEngine;

namespace ET.Client
{
    public static class CoroutineHelper
    {
        // 有了这个方法，就可以直接await Unity的AsyncOperation了
        public static async ETTask GetAwaiter(this AsyncOperation asyncOperation)
        {
            ETTask task = ETTask.Create(true);
            asyncOperation.completed += _ => { task.SetResult(); };
            await task;
        }
    }
}