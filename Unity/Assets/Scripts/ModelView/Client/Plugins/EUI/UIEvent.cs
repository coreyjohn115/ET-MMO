using System;
using System.Collections.Generic;

namespace ET.Client
{
    [Code]
    public class UIEvent: Singleton<UIEvent>, ISingletonAwake
    {
        public Dictionary<WindowID, IUIEventHandler> UIEventHandlers;

        public bool IsClick { get; set; }

        public void Awake()
        {
            UIEventHandlers = new Dictionary<WindowID, IUIEventHandler>();
            foreach (var v in CodeTypes.Instance.GetTypes(typeof (UIEventAttribute)))
            {
                UIEventAttribute attr = v.GetCustomAttributes(typeof (UIEventAttribute), false)[0] as UIEventAttribute;
                UIEventHandlers.Add(attr.WindowID, Activator.CreateInstance(v) as IUIEventHandler);
            }
        }

        public IUIEventHandler GetUIEventHandler(WindowID windowID)
        {
            if (UIEventHandlers.TryGetValue(windowID, out IUIEventHandler handler))
            {
                return handler;
            }

            Log.Error($"windowId : {windowID} is not have any uiEvent");
            return null;
        }

        public void SetUIClick(bool click)
        {
            IsClick = click;
        }
    }
}