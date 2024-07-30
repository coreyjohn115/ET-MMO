using System.Collections.Generic;

namespace ET
{
    public partial class WindowCategory
    {
        private Dictionary<string, Window> windows = new Dictionary<string, Window>();

        public Window GetWindow(string type)
        {
            return windows[type];
        }
        
        public override void EndInit()
        {
            foreach (var w in this.dict)
            {
                this.windows.Add(w.Value.Type, w.Value);
            }
        }
    }
}