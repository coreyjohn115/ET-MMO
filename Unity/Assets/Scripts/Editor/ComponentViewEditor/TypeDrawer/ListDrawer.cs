using System;
using System.Collections.Generic;
using UnityEditor;

namespace ET
{
    [TypeDrawer]
    public class ListLongDrawer: ITypeDrawer
    {
        public bool HandlesType(Type type)
        {
            return type == typeof (List<long>);
        }

        public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            List<long> parames = value as List<long>;

            EditorGUILayout.LabelField($"{memberName}:");
            foreach (var s in parames)
            {
                EditorGUILayout.LongField(s);
            }

            return value;
        }
    }

    [TypeDrawer]
    public class ListIntDrawer: ITypeDrawer
    {
        public bool HandlesType(Type type)
        {
            return type == typeof (List<int>);
        }

        public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            List<int> parames = value as List<int>;

            EditorGUILayout.LabelField($"{memberName}:");
            foreach (var s in parames)
            {
                EditorGUILayout.IntField(s);
            }

            return value;
        }
    }
}