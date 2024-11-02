using System;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    [CustomEditor(typeof (RectTransform))]
    [CanEditMultipleObjects]
    public class GUIAlignment: DecoratorEditor
    {
        /// <summary>
        /// 继承自类DecoratorEditor，做反射获取RectTransformEditor内属性、方法
        /// </summary>
        public GUIAlignment(): base("RectTransformEditor")
        {
        }

        /// <summary>
        /// 修改Inspector面板
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", GUILayout.Width(60)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        Undo.RecordObject(rect, rect.name);

                        var position = rect.anchoredPosition;
                        var x = position.x.ToString("#.0");
                        var y = position.y.ToString("#.0");
                        rect.anchoredPosition = new Vector2(Convert.ToSingle(x), Convert.ToSingle(y));

                        var size = rect.sizeDelta;
                        x = size.x.ToString("#.0");
                        y = size.y.ToString("#.0");
                        rect.sizeDelta = new Vector2(Convert.ToSingle(x), Convert.ToSingle(y));
                    }
                }
            }

            if (GUILayout.Button("Position", GUILayout.Width(60)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        Undo.RecordObject(rect, rect.name);
                        rect.anchoredPosition = Vector3.zero;
                    }
                }
            }

            if (GUILayout.Button("Rotation", GUILayout.Width(60)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        Undo.RecordObject(rect, rect.name);
                        rect.eulerAngles = Vector3.zero;
                    }
                }
            }

            if (GUILayout.Button("Scale", GUILayout.Width(60)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        Undo.RecordObject(rect, rect.name);
                        rect.localScale = Vector3.one;
                    }
                }
            }

            if (GUILayout.Button("Pivot", GUILayout.Width(60)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        Undo.RecordObject(rect, rect.name);
                        rect.pivot = Vector3.one * 0.5f;
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", GUILayout.Width(60)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        Undo.RecordObject(rect, rect.name);
                        var pos = rect.anchoredPosition;
                        var size = rect.sizeDelta;
                        var posX = pos.x.ToString("0000.0");
                        var posY = pos.y.ToString("0000.0");
                        var sizeX = size.x.ToString("0000.0");
                        var sizeY = size.y.ToString("0000.0");

                        rect.anchoredPosition = new Vector2(Convert.ToSingle(posX), Convert.ToSingle(posY));
                        rect.sizeDelta = new Vector2(Convert.ToSingle(sizeX), Convert.ToSingle(sizeY));

                        rect.localScale = Vector3.one;
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy", GUILayout.Width(60)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        var pos = rect.anchoredPosition;
                        var size = rect.sizeDelta;
                        var posX = pos.x.ToString("0000.0");
                        var posY = pos.y.ToString("0000.0");
                        var sizeX = size.x.ToString("0000.0");
                        var sizeY = size.y.ToString("0000.0");

                        GUIUtility.systemCopyBuffer = "custom->" + posX + ":" + posY + ":" + sizeX + ":" + sizeY;
                    }
                }
            }

            if (GUILayout.Button("Paste", GUILayout.Width(60)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        Undo.RecordObject(rect, rect.name);
                        var buffer = GUIUtility.systemCopyBuffer;
                        if (buffer.StartsWith("custom->"))
                        {
                            var values = buffer.Substring(8).Split(':');
                            var posX = values[0];
                            var posY = values[1];
                            var sizeX = values[2];
                            var sizeY = values[3];

                            rect.anchoredPosition = new Vector2(Convert.ToSingle(posX), Convert.ToSingle(posY));
                            rect.sizeDelta = new Vector2(Convert.ToSingle(sizeX), Convert.ToSingle(sizeY));
                        }
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(); //横向
            EditorGUILayout.LabelField("AlignParent");

            EditorGUILayout.BeginVertical(); //开始绘制九宫格
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal(); //first row
            if (GUILayout.Button("┏", GUILayout.Width(25)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        if (rect.parent != null)
                        {
                            RectTransform parent = (RectTransform)rect.parent;
                            SetPos(rect, parent, 1);
                        }
                    }
                }
            }

            if (GUILayout.Button("┳", GUILayout.Width(25)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        if (rect.parent != null)
                        {
                            RectTransform parent = (RectTransform)rect.parent;
                            SetPos(rect, parent, 2);
                        }
                    }
                }
            }

            if (GUILayout.Button("┓", GUILayout.Width(25)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        if (rect.parent != null)
                        {
                            RectTransform parent = (RectTransform)rect.parent;
                            SetPos(rect, parent, 3);
                        }
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(); //second row
            if (GUILayout.Button("┣", GUILayout.Width(25)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        if (rect.parent != null)
                        {
                            RectTransform parent = (RectTransform)rect.parent;
                            SetPos(rect, parent, 4);
                        }
                    }
                }
            }

            if (GUILayout.Button("╋", GUILayout.Width(25)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        if (rect.parent != null)
                        {
                            RectTransform parent = (RectTransform)rect.parent;
                            SetPos(rect, parent, 5);
                        }
                    }
                }
            }

            if (GUILayout.Button("┫", GUILayout.Width(25)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        if (rect.parent != null)
                        {
                            RectTransform parent = (RectTransform)rect.parent;
                            SetPos(rect, parent, 6);
                        }
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(); //third row
            if (GUILayout.Button("┗", GUILayout.Width(25)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        if (rect.parent != null)
                        {
                            RectTransform parent = (RectTransform)rect.parent;
                            SetPos(rect, parent, 7);
                        }
                    }
                }
            }

            if (GUILayout.Button("┻", GUILayout.Width(25)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        if (rect.parent != null)
                        {
                            RectTransform parent = (RectTransform)rect.parent;
                            SetPos(rect, parent, 8);
                        }
                    }
                }
            }

            if (GUILayout.Button("┛", GUILayout.Width(25)))
            {
                foreach (var o in targets)
                {
                    if (o is RectTransform rect)
                    {
                        if (rect.parent != null)
                        {
                            RectTransform parent = (RectTransform)rect.parent;
                            SetPos(rect, parent, 9);
                        }
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 设置位置 1 2 3 4 5 6 7 8 9
        /// </summary>
        /// <param name="self">操作的UI</param>
        /// <param name="parent">父节点</param>
        /// <param name="type">九宫格位置</param>
        private void SetPos(RectTransform self, RectTransform parent, int type)
        {
            Undo.RecordObject(self, self.name);
            Vector2 max = self.anchorMax;
            Vector2 min = self.anchorMin;
            Vector2 pos = self.anchoredPosition; //坐标点相对锚点位置

            Vector2 p_middle = parent.sizeDelta * 0.5f; //父节点的size的一半
            p_middle.x = p_middle.x * parent.localScale.x;
            p_middle.y = p_middle.y * parent.localScale.y; //考虑到缩放
            Vector2 s_middle = self.sizeDelta * 0.5f;
            s_middle.x = s_middle.x * self.localScale.x;
            s_middle.y = s_middle.y * self.localScale.y;

            self.anchorMax = Vector2.one * 0.5f; //重置锚点位置为居中
            self.anchorMin = Vector2.one * 0.5f;
            self.anchoredPosition = Vector2.zero; //重置UI位置为正中

            switch (type)
            {
                case 1:
                    pos.x = -(p_middle.x - s_middle.x); //因为unity的坐标采用左下角为坐标元点，故取负值
                    pos.y = (p_middle.y - s_middle.y);
                    break;

                case 2:
                    pos.x = 0;
                    pos.y = (p_middle.y - s_middle.y);
                    break;

                case 3:
                    pos.x = (p_middle.x - s_middle.x);
                    pos.y = (p_middle.y - s_middle.y);
                    break;

                case 4:
                    pos.x = -(p_middle.x - s_middle.x);
                    pos.y = 0;
                    break;

                case 5:
                    pos.x = 0;
                    pos.y = 0;
                    break;

                case 6:
                    pos.x = (p_middle.x - s_middle.x);
                    pos.y = 0;
                    break;

                case 7:
                    pos.x = -(p_middle.x - s_middle.x);
                    pos.y = -(p_middle.y - s_middle.y);
                    break;

                case 8:
                    pos.x = 0;
                    pos.y = -(p_middle.y - s_middle.y);
                    break;

                case 9:
                    pos.x = (p_middle.x - s_middle.x);
                    pos.y = -(p_middle.y - s_middle.y);
                    break;
            }

            self.anchoredPosition = pos;
        }
    }
}