using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// UI界面对应路径
    /// </summary>
    [Code]
    public class UIPath: Singleton<UIPath>, ISingletonAwake
    {
        public Dictionary<int, string> WindowPrefabPath { get; private set; }

        public Dictionary<string, int> WindowTypeIdDict { get; private set; }

        public void Awake()
        {
            WindowPrefabPath = new Dictionary<int, string>();
            WindowTypeIdDict = new Dictionary<string, int>();
            foreach (WindowID windowID in Enum.GetValues(typeof (WindowID)))
            {
                string dlgName = windowID.ToString().Split('_')[1];
                WindowPrefabPath.Add((int) windowID, dlgName);
                WindowTypeIdDict.Add(dlgName, (int) windowID);
            }
        }
    }
}