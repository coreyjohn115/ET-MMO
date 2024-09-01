using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public static class WaitTypeError
    {
        public const int Success = 0;
        public const int Destroy = 1;
        public const int Cancel = 2;
        public const int Timeout = 3;
    }

    public interface IWaitType
    {
        int Error { get; set; }
    }

    [EntitySystemOf(typeof (ObjectWait))]
    [FriendOf(typeof (ObjectWait))]
    public static partial class ObjectWaitSystem
    {
        [EntitySystem]
        private static void Awake(this ObjectWait self)
        {
            self.tcsDict.Clear();
        }

        [EntitySystem]
        private static void Destroy(this ObjectWait self)
        {
            foreach (var p in self.tcsDict)
            {
                foreach (object v in p.Value)
                {
                    ((IDestroyRun)v).SetResult();
                }
            }

            self.tcsDict.Clear();
        }

        private interface IDestroyRun
        {
            void SetResult();
        }

        private class ResultCallback<K>: IDestroyRun where K : struct, IWaitType
        {
            private ETTask<K> tcs;

            public ResultCallback()
            {
                this.tcs = ETTask<K>.Create(true);
            }

            public bool IsDisposed
            {
                get
                {
                    return this.tcs == null;
                }
            }

            public ETTask<K> Task => this.tcs;

            public void SetResult(K k)
            {
                var t = tcs;
                this.tcs = null;
                t.SetResult(k);
            }

            public void SetResult()
            {
                var t = tcs;
                this.tcs = null;
                t.SetResult(new K() { Error = WaitTypeError.Destroy });
            }
        }

        public static async ETTask<T> Wait<T>(this ObjectWait self) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T>();
            ETCancellationToken cancellationToken = await ETTaskHelper.GetContextAsync<ETCancellationToken>();
            self.Add(typeof (T), tcs);

            T ret;
            try
            {
                cancellationToken?.Add(CancelAction);
                ret = await tcs.Task;
            }
            finally
            {
                cancellationToken?.Remove(CancelAction);
            }

            return ret;

            void CancelAction()
            {
                self.Notify(new T() { Error = WaitTypeError.Cancel });
            }
        }

        public static void Notify<T>(this ObjectWait self, T obj) where T : struct, IWaitType
        {
            Type type = typeof (T);
            if (!self.tcsDict.TryGetValue(type, out var tcsList) || tcsList.Count == 0)
            {
                return;
            }

            foreach (var tcs in tcsList)
            {
                ((ResultCallback<T>)tcs).SetResult(obj);
            }

            tcsList.Clear();
        }

        private static void Add(this ObjectWait self, Type type, object obj)
        {
            if (self.tcsDict.TryGetValue(type, out var list))
            {
                list.Add(obj);
            }
            else
            {
                self.tcsDict.Add(type, new List<object> { obj });
            }
        }
    }

    [ComponentOf]
    public class ObjectWait: Entity, IAwake, IDestroy
    {
        public Dictionary<Type, List<object>> tcsDict = new();
    }
}