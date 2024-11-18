using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ET.Client
{
    [Code]
    public class ActionSingleton: Singleton<ActionSingleton>, ISingletonAwake
    {
        private Dictionary<string, Func<object>> actionTypeDic;

        public void Awake()
        {
            actionTypeDic = new Dictionary<string, Func<object>>();
            foreach (var v in CodeTypes.Instance.GetTypes(typeof (ActionAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (ActionAttribute), false)[0] as ActionAttribute;

                Func<object> func = Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(v), typeof (object))).Compile;
                actionTypeDic.Add(attr.ActionType, func);
            }
        }

        public AAction GetAction(string name)
        {
            if (!actionTypeDic.TryGetValue(name, out var t))
            {
                Thrower.Throw($"Action {name} not found");
            }

            if (t == null)
            {
                return default;
            }

            return t() as AAction;
        }
    }
}