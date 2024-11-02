using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 层次排序
    /// </summary>
    public static class PriorityHelper
    {
        [MenuItem("Tools/层次/最里层 " + Configure.AltStr + Configure.ShiftStr + "z")]
        public static void MoveToTopWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                curSelect.SetAsFirstSibling();
            }
        }

        [MenuItem("Tools/层次/最外层 " + Configure.AltStr + Configure.ShiftStr + "c")]
        public static void MoveToBottomWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                curSelect.SetAsLastSibling();
            }
        }

        [MenuItem("Tools/层次/往里挤 " + Configure.AltStr + "z")]
        public static void MoveUpWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                int curIndex = curSelect.GetSiblingIndex();
                if (curIndex > 0)
                {
                    curSelect.SetSiblingIndex(curIndex - 1);
                }
            }
        }

        [MenuItem("Tools/层次/往外挤 " + Configure.AltStr + "c")]
        public static void MoveDownWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                int curIndex = curSelect.GetSiblingIndex();
                int childNum = curSelect.parent.childCount;
                if (curIndex < childNum - 1)
                {
                    curSelect.SetSiblingIndex(curIndex + 1);
                }
            }
        }
    }
}