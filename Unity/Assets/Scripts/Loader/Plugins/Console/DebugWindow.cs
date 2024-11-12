using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    public abstract class DebugWindowBase
    {
        protected Rect windowRect;
        protected bool isInEditor;
        private float scale = 1.8f;

        private bool controlSize;
        private Vector2 startPos;
        private Vector2 startSize;

        public void Init(Rect winRect, bool inEditor)
        {
            this.windowRect = new Rect(winRect.position / this.scale, winRect.size / this.scale);
            this.isInEditor = inEditor;
            if (this.isInEditor)
            {
                this.windowRect = winRect;
            }
        }

        public void Draw()
        {
            if (this.isInEditor)
            {
                OnDrawWindow(0);
            }
            else
            {
                GUI.matrix = Matrix4x4.Scale(new Vector3(this.scale, this.scale, 1f));
                GUI.Window(0, this.windowRect, OnDrawWindow, "Debug");
                GUI.matrix = Matrix4x4.Scale(new Vector3(1, 1, 1f));
            }
        }

        protected abstract void OnDrawWindow(int id);
    }

    public class DebugWindow: MonoBehaviour
    {
        private float fps;
        private float lastRefresh;
        private bool beginDrag;
        private Vector2 beginPos;
        private Vector2 dragPos;
        private bool isClick;
        private const float scale = 1.5f;

        private Vector2 minBtnPos;
        private Image block;
        private DebugWindowBase[] windows;
        private int showIndex = 0;

        private readonly Type[] windowTypes =
        {
            typeof (DebugWindowLog), typeof (DebugWindowTools), typeof (DebugWindowServerCMD), typeof (UnityObjectViewer)
        };

        private readonly string[] showNames = { "X", "Log", "工具", "协议", "属性" };

        private void Awake()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        private static void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            ConsoleLogs.Instance.Add(condition, stacktrace, type);
        }

        private void Start()
        {
            this.windows = new DebugWindowBase[this.windowTypes.Length];
            for (int i = 0; i < this.windowTypes.Length; i++)
            {
                Type windowType = this.windowTypes[i];
                this.windows[i] = Activator.CreateInstance(windowType) as DebugWindowBase;
                this.windows[i].Init(new Rect(20, 40, Screen.width - 40, Screen.height - 60), false);
            }

            this.minBtnPos = new Vector2((Screen.width / 2f - 25), 10) / scale;

            Canvas can = new GameObject("BlockCanvas").AddComponent<Canvas>();
            can.vertexColorAlwaysGammaSpace = true;
            can.gameObject.layer = LayerMask.NameToLayer("UI");
            can.transform.SetParent(this.transform);
            can.worldCamera = GameObject.Find("Global/UICamera").GetComponent<Camera>();
            can.renderMode = RenderMode.ScreenSpaceCamera;
            can.sortingOrder = 30000;

            can.gameObject.AddComponent<GraphicRaycaster>();

            this.block = new GameObject("Block").AddComponent<Image>();
            this.block.gameObject.layer = LayerMask.NameToLayer("UI");
            this.block.color = new Color(0, 0, 0, 0.6f);
            this.block.transform.SetParent(can.transform, false);
            this.block.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            this.block.gameObject.SetActive(false);
        }

        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));
            this.block.gameObject.SetActive(this.showIndex != 0);
            if (this.showIndex != 0)
            {
                var win = this.windows[this.showIndex - 1];
                win.Draw();
                this.DrawTab();
            }
            else
            {
                var area = new Rect(this.minBtnPos, new Vector2(50, 20));
                GUI.Box(area, this.fps.ToString("f0"));
                Vector2 pos = Event.current.mousePosition;
                switch (Event.current.type)
                {
                    case EventType.MouseDown:
                        if (Event.current.button == 0 && area.Contains(pos))
                        {
                            this.beginDrag = true;
                            this.beginPos = pos;
                            this.dragPos = this.minBtnPos;
                            this.isClick = true;
                        }

                        break;
                    case EventType.MouseDrag:
                        if (this.beginDrag)
                        {
                            this.minBtnPos = this.dragPos + (pos - this.beginPos);
                            this.isClick = false;
                        }

                        break;
                    case EventType.MouseUp:
                        if (this.isClick)
                        {
                            this.showIndex = 1;
                            this.isClick = false;
                        }

                        this.beginDrag = false;
                        break;
                }
            }
        }

        private void DrawTab()
        {
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));
            var half = 60 * (this.windowTypes.Length);
            this.showIndex = GUI.Toolbar(new Rect((Screen.width / 2f - half) / scale, 10 / scale, 60 * this.windows.Length, 20),
                this.showIndex, this.showNames);
        }

        private void Update()
        {
            if (Time.time - this.lastRefresh > 1)
            {
                this.fps = 1 / Time.deltaTime;
                this.lastRefresh = Time.time;
            }

#if UNITY_EDITOR

            List<RaycastResult> m_RaycastResult = new List<RaycastResult>();
            if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftControl))
            {
                PointerEventData data = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                data.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                UnityEngine.EventSystems.EventSystem.current.RaycastAll(data, m_RaycastResult);
                if (m_RaycastResult.Count > 0)
                {
                    UnityEditor.EditorGUIUtility.PingObject(m_RaycastResult[0].gameObject);
                }
            }
#endif
        }
    }
}