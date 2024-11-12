using UnityEngine;

namespace ET.Client
{
    [Code]
    public class DebugSingleton: Singleton<DebugSingleton>, ISingletonAwake
    {
        private GameObject go;

        public void Awake()
        {
            if (AppSetting.Instance.Debug || Define.IsDebug)
            {
                go = new GameObject("Debug", typeof (DebugWindow));
                go.transform.SetParent(Global.Instance.GlobalTrans);
            }
        }

        public void OpenLog()
        {
            if (this.go)
            {
                return;
            }

            go = new GameObject("Debug", typeof (DebugWindow));
            go.transform.SetParent(Global.Instance.GlobalTrans);
        }
    }
}