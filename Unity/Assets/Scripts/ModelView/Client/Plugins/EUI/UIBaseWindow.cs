using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof (UIComponent))]
    public class UIBaseWindow: Entity, IAwake, IDestroy
    {
        public WindowCoreData WindowData => this.GetComponent<WindowCoreData>();

        /// <summary>
        /// 是否显示遮罩
        /// </summary>
        public bool ShowMask { get; set; } = true;

        /// <summary>
        /// 是否预加载窗口
        /// </summary>
        public bool IsPreLoad
        {
            get
            {
                return this.UIPrefabGameObject != null;
            }
        }

        /// <summary>
        /// UI窗口根节点
        /// </summary>
        public RectTransform UITransform
        {
            get
            {
                if (this.UIPrefabGameObject)
                {
                    return (RectTransform)this.UIPrefabGameObject.transform;
                }

                return null;
            }
        }

        public WindowID WindowID
        {
            get
            {
                if (this.m_windowID == WindowID.Win_Invaild)
                {
                    Log.Error("window id is " + WindowID.Win_Invaild);
                }

                return m_windowID;
            }
            set
            {
                m_windowID = value;
            }
        }

        public bool IsInStackQueue { get; set; }

        public WindowID PreWindowID
        {
            get
            {
                return m_preWindowID;
            }
            set
            {
                m_preWindowID = value;
            }
        }

        public WindowID m_preWindowID = WindowID.Win_Invaild;
        public WindowID m_windowID = WindowID.Win_Invaild;
        public GameObject UIPrefabGameObject = null;
        public Dictionary<string, int> spriteRefCount = new();
    }
}