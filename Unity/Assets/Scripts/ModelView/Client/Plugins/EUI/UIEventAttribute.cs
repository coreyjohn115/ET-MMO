using System;

namespace ET.Client
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UIEventAttribute: BaseAttribute
    {
        public WindowID WindowID
        {
            get;
        }

        public UIEventAttribute(WindowID windowID)
        {
            this.WindowID = windowID;
        }
    }
}