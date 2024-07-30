using System;
using System.Collections.Generic;

namespace ET.Client
{
    [Code]
    public class UIIComSingleton: Singleton<UIIComSingleton>, ISingletonAwake
    {
        private Dictionary<string, AUIComHandler> allHandlers = new();

        public void Awake()
        {
            HashSet<Type> types = CodeTypes.Instance.GetTypes(typeof (UIComAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (UIComAttribute), false);
                foreach (object attr in attrs)
                {
                    UIComAttribute comAttribute = (UIComAttribute)attr;
                    AUIComHandler obj = (AUIComHandler)Activator.CreateInstance(type);
                    allHandlers[comAttribute.Name] = obj;
                }
            }
        }

        public void Show(string name, Entity uiCom)
        {
            if (!allHandlers.TryGetValue(name, out AUIComHandler handler))
            {
                return;
            }

            try
            {
                handler.Show(uiCom);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}