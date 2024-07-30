using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// 控制<see cref="Text"/>字间距
    /// </summary>
    [AddComponentMenu("UI/Effects/TextSpacing")]
    public class TextSpacing: BaseMeshEffect
    {
        #region Public Methods

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive() || vh.currentVertCount == 0)
            {
                return;
            }

            var text = GetComponent<Text>();

            if (text == null)
            {
                Debug.LogError("Missing Text component");
                return;
            }

            // 水平对齐方式
            HorizontalAligmentType alignment;
            if (text.alignment == TextAnchor.LowerLeft ||
                text.alignment == TextAnchor.MiddleLeft ||
                text.alignment == TextAnchor.UpperLeft)
            {
                alignment = HorizontalAligmentType.Left;
            }
            else if (text.alignment == TextAnchor.LowerCenter ||
                     text.alignment == TextAnchor.MiddleCenter ||
                     text.alignment == TextAnchor.UpperCenter)
            {
                alignment = HorizontalAligmentType.Center;
            }
            else
            {
                alignment = HorizontalAligmentType.Right;
            }

            var vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);
            // var indexCount = vh.currentIndexCount;

            var lineTexts = text.text.Split('\n');

            var lines = new Line[lineTexts.Length];

            // 根据lines数组中各个元素的长度计算每一行中第一个点的索引，每个字、字母、空母均占6个点
            for (var i = 0; i < lines.Length; i++)
            {
                // 除最后一行外，vertexs对于前面几行都有回车符占了6个点
                if (i == 0)
                {
                    lines[i] = new Line(0, lineTexts[i].Length + 1);
                }
                else if (i > 0 && i < lines.Length - 1)
                {
                    lines[i] = new Line(lines[i - 1].EndVertexIndex + 1,
                        lineTexts[i].Length + 1);
                }
                else
                {
                    lines[i] = new Line(lines[i - 1].EndVertexIndex + 1,
                        lineTexts[i].Length);
                }
            }

            UIVertex vt;

            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = lines[i].StartVertexIndex; j <= lines[i].EndVertexIndex; j++)
                {
                    if (j < 0 || j >= vertexs.Count)
                    {
                        continue;
                    }

                    vt = vertexs[j];

                    var charCount = lines[i].EndVertexIndex - lines[i].StartVertexIndex;
                    if (i == lines.Length - 1)
                    {
                        charCount += 6;
                    }

                    if (alignment == HorizontalAligmentType.Left)
                    {
                        vt.position += new Vector3(spacing * ((j - lines[i].StartVertexIndex) / 6),
                            0,
                            0);
                    }
                    else if (alignment == HorizontalAligmentType.Right)
                    {
                        vt.position += new Vector3(spacing * (-(charCount - j + lines[i].StartVertexIndex) /
                                6 + 1),
                            0,
                            0);
                    }
                    else if (alignment == HorizontalAligmentType.Center)
                    {
                        var offset = (charCount / 6) % 2 == 0? 0.5f : 0f;
                        vt.position += new Vector3(spacing * ((j - lines[i].StartVertexIndex) /
                                6 - charCount / 12 + offset),
                            0,
                            0);
                    }

                    vertexs[j] = vt;
                    // 以下注意点与索引的对应关系
                    if (j % 6 <= 2)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
                    }

                    if (j % 6 == 4)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                    }
                }
            }
        }

        #endregion

        #region Internal Fields

        [SerializeField]
        public float spacing = 1f;

        #endregion

        #region Internal Declarations

        private enum HorizontalAligmentType
        {
            Left,
            Center,
            Right
        }

        private struct Line
        {
            #region Public Properties

            /// <summary>
            /// 起点索引
            /// </summary>
            public int StartVertexIndex { get; }

            /// <summary>
            /// 终点索引
            /// </summary>
            public int EndVertexIndex { get; }

            /// <summary>
            /// 该行占的点数目
            /// </summary>
            public int VertexCount { get; }

            #endregion

            #region Public Methods

            public Line(int startVertexIndex, int length)
            {
                StartVertexIndex = startVertexIndex;
                EndVertexIndex = length * 6 - 1 + startVertexIndex;
                VertexCount = length * 6;
            }

            #endregion
        }

        #endregion
    }
}