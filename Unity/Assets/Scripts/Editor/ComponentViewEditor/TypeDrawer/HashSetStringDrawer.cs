using System;
using System.Collections.Generic;
using UnityEditor;

namespace ET
{
    [TypeDrawer]
    public class HashSetStringDrawer: ITypeDrawer
    {
        public bool HandlesType(Type type)
        {
            return type == typeof (HashSet<string>);
        }

        public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            HashSet<string> parames = value as HashSet<string>;

            EditorGUILayout.LabelField($"{memberName}:");
            foreach (var s in parames)
            {
                EditorGUILayout.TextField(s);
            }

            return value;
        }
    }
}

