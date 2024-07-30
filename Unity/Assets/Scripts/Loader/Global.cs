using UnityEngine;

namespace ET
{
    [Code]
    public class Global: Singleton<Global>, ISingletonAwake
    {
        public Transform GlobalTrans { get; private set; }

        public Transform Unit { get; private set; }

        public Transform UI { get; private set; }

        public Transform Pool { get; private set; }

        public Transform CameraRoot { get; private set; }

        public RectTransform NormalRoot { get; private set; }

        public RectTransform PopUpRoot { get; private set; }

        public RectTransform FixedRoot { get; private set; }

        public RectTransform OtherRoot { get; private set; }

        /// <summary>
        /// UI摄像机
        /// </summary>
        public Camera UICamera { get; private set; }

        /// <summary>
        /// 主摄像机
        /// </summary>
        public Camera MainCamera { get; private set; }

        /// <summary>
        /// 游戏全局配置
        /// </summary>
        public GlobalConfig GlobalConfig { get; private set; }

        /// <summary>
        /// 摄像机配置
        /// </summary>
        public CameraScriptObject CameraConfig { get; private set; }
        
        public GameScriptObject GameConfig { get; private set; }

        private ReferenceCollector collector;

        public T Get<T>(string name) where T : class
        {
            return this.collector.Get<T>(name);
        }

        public void Awake()
        {
            GlobalTrans = GameObject.Find("/Global").transform;
            collector = this.GlobalTrans.GetComponent<ReferenceCollector>();

            UI = collector.Get<Transform>("UI");
            Pool = collector.Get<Transform>("Pool");
            Unit = collector.Get<Transform>("Unit");
            CameraRoot = collector.Get<Transform>("Camera");
            UICamera = collector.Get<Camera>("UICamera");
            MainCamera = collector.Get<Camera>("MainCamera");

            NormalRoot = this.UI.Find("NormalRoot").transform as RectTransform;
            PopUpRoot = this.UI.Find("PopUpRoot").transform as RectTransform;
            FixedRoot = this.UI.Find("FixedRoot").transform as RectTransform;
            OtherRoot = this.UI.Find("OtherRoot").transform as RectTransform;

            GlobalConfig = Resources.Load<GlobalConfig>(nameof (GlobalConfig));
            CameraConfig = Resources.Load<CameraScriptObject>(nameof (CameraConfig));
            GameConfig = Resources.Load<GameScriptObject>(nameof (GameConfig));
        }
    }
}