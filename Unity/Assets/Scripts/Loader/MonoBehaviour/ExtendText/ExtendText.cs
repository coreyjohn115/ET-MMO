using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// 支持本地化的文本
    /// </summary>
    public class ExtendText : Text
    {
        public enum GradientStyle
        {
            Local,
            Global
        }

        public enum GradientDirection
        {
            Vertical,
            Horizontal
        }

        [SerializeField]
        private bool m_Shrink;

        [SerializeField]
        private bool m_Gradient;

        [SerializeField]
        private Color m_GradientColor = Color.black;

        [SerializeField]
        private GradientStyle m_GradientStyle = GradientStyle.Local;

        [SerializeField]
        private GradientDirection m_GradientDirection = GradientDirection.Vertical;

        [SerializeField]
        private bool m_AlignEnd;

        [SerializeField]
        private bool m_Ellipsis;

        /// <summary>
        /// 渐变色从Color线下渐变至GradientColor
        /// </summary>
        public Color GradientColor
        {
            get => m_GradientColor;
            set
            {
                if (m_GradientColor == value)
                    return;

                m_GradientColor = value;
                if (m_Gradient)
                    SetVerticesDirty();
            }
        }

        /// <summary>
        /// 渐变
        /// </summary>
        public bool Gradient
        {
            get => m_Gradient;
            set
            {
                if (m_Gradient == value)
                    return;

                m_Gradient = value;
                if (!supportRichText)
                    SetVerticesDirty();
            }
        }

        public int VisibleLines => cachedTextGenerator.lineCount;

        private void Shrink()
        {
            TextGenerationSettings settings = GetGenerationSettings(rectTransform.rect.size);
            if (m_Shrink)
            {
                settings.resizeTextForBestFit = false;
                Rect rect = rectTransform.rect;
                var height = cachedTextGenerator.GetPreferredHeight(text, settings);
                if (height > rect.height)
                {
                    var s = settings.fontSize;
                    for (int i = s; i >= 0; --i)
                    {
                        settings.fontSize = i;
                        var h = cachedTextGenerator.GetPreferredHeight(text, settings);
                        if (h <= rect.height) break;
                    }
                }

                cachedTextGenerator.Populate(text, settings);
            }
        }

        private void Ellipsis()
        {
            if (m_Ellipsis)
            {
                TextGenerationSettings settings = GetGenerationSettings(rectTransform.rect.size);
                settings.resizeTextForBestFit = false;
                string tempText = text;
                if (cachedTextGenerator.lineCount == 1)
                    tempText = tempText.Replace(" ", "\u00A0");
                cachedTextGenerator.Populate(tempText, settings);
                var characterCountVisible = cachedTextGenerator.characterCountVisible;
                if (characterCountVisible <= 0) return;
                if (tempText.Length > characterCountVisible)
                {
                    tempText = text.Substring(0, characterCountVisible - 1);
                    tempText += "...";
                }
                cachedTextGenerator.Populate(tempText, settings);
            }
        }

        private const byte _quadVertCount = 4;
        private readonly List<UIVertex> _leftVerts = new List<UIVertex>(16);
        private readonly List<UIVertex> _rightVerts = new List<UIVertex>(16);
        private readonly UIVertex[] _quadVerts = new UIVertex[4];

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (font == null) return;

            #region 源码。本来想把源码搞掉,修改shrink可能会对以前有一点点影响,还是算了

            // We don't care if we the font Texture changes while we are doing our Update.
            // The end result of cachedTextGenerator will be valid for this instance.
            // Otherwise we can get issues like Case 619238.
            m_DisableFontTextureRebuiltCallback = true;

            Vector2 extents = rectTransform.rect.size;

            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.PopulateWithErrors(text, settings, gameObject);
            Shrink();
            Ellipsis();
            // Apply the offset to the vertices
            IList<UIVertex> verts = cachedTextGenerator.verts;
            float unitsPerPixel = 1 / pixelsPerUnit;
            //Last 4 verts are always a new line... (\n)
            int vertCount = verts.Count;
            // We have no verts to process just return (case 1037923)
            if (vertCount <= 0)
            {
                toFill.Clear();
                return;
            }

            Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
            roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
            toFill.Clear();
            if (roundingOffset != Vector2.zero)
            {
                for (int i = 0; i < vertCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    _quadVerts[tempVertsIndex] = verts[i];
                    _quadVerts[tempVertsIndex].position *= unitsPerPixel;
                    _quadVerts[tempVertsIndex].position.x += roundingOffset.x;
                    _quadVerts[tempVertsIndex].position.y += roundingOffset.y;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(_quadVerts);
                }
            }
            else
            {
                for (int i = 0; i < vertCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    _quadVerts[tempVertsIndex] = verts[i];
                    _quadVerts[tempVertsIndex].position *= unitsPerPixel;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(_quadVerts);
                }
            }

            #endregion

            // 渐变处理
            if (!supportRichText && m_Gradient)
            {
                UIVertex vertex = new UIVertex();
                if (m_GradientStyle == GradientStyle.Local)
                {
                    for (var i = 0; i < toFill.currentVertCount; i++)
                    {
                        toFill.PopulateUIVertex(ref vertex, i);
                        var index = i & 3;
                        switch (m_GradientDirection)
                        {
                            case GradientDirection.Vertical when index == 2 || index == 3:
                                vertex.color = m_GradientColor;
                                break;
                            case GradientDirection.Horizontal when index == 1 || index == 2:
                                vertex.color = m_GradientColor;
                                break;
                        }

                        toFill.SetUIVertex(vertex, i);
                    }
                }
                else // Global
                {
                    var isHorizontal = m_GradientDirection == GradientDirection.Horizontal;

                    float min = isHorizontal ? rectTransform.rect.xMax : rectTransform.rect.yMax;
                    float max = isHorizontal ? rectTransform.rect.xMin : rectTransform.rect.yMin;
                    for (var i = 0; i < toFill.currentVertCount; ++i)
                    {
                        toFill.PopulateUIVertex(ref vertex, i);
                        min = Mathf.Min(min, isHorizontal ? vertex.position.x : vertex.position.y);
                        max = Mathf.Max(max, isHorizontal ? vertex.position.x : vertex.position.y);
                    }

                    for (var i = 0; i < toFill.currentVertCount; ++i)
                    {
                        toFill.PopulateUIVertex(ref vertex, i);
                        var t = (isHorizontal ? vertex.position.x - min : vertex.position.y - min) / (max - min);
                        vertex.color = Color32.Lerp(color, GradientColor, isHorizontal ? t : 1 - t); // 左=>右 上=>下
                        toFill.SetUIVertex(vertex, i);
                    }
                }
            }

            // 两端对齐
            if (m_AlignEnd && toFill.currentVertCount > 0)
            {
                //print($"{cachedTextGenerator.characterCount} {cachedTextGenerator.characterCountVisible} {cachedTextGenerator.vertexCount}");
                UIVertex vertex = new UIVertex();
                var isLeft = true;
                for (int i = 0; i < toFill.currentVertCount / _quadVertCount; i++)
                {
                    var quadIndex0 = i * _quadVertCount;
                    toFill.PopulateUIVertex(ref vertex, quadIndex0);
//#if UNITY_2018_4_36
//                    var vertPos = vertex.position;
//                    while (currLine.height == 0 || vertPos.y < currLine.topY - currLine.height || vertPos.y > currLine.topY)
//                    {
//                        if (currLine.height != 0) PartVertex(toFill); // 新的一行分开左右顶点

//                        isLeft = true;
//                        print("seek to a line");
//                        foreach (var line in cachedTextGenerator.lines)
//                        {
//                            if (vertPos.y <= line.topY && vertPos.y >= line.topY - line.height)
//                            {
//                                currLine = line;
//                                break;
//                            }
//                        }
//                    }
//#else
                    if (text[i] == '\n')
                    {
                        if (i != 0) PartVertex(toFill);
                        isLeft = true;
                    }

//#endif

                    for (int j = 0; j < _quadVertCount; j++) //一个字的片
                    {
                        var index = i * _quadVertCount + j;
                        toFill.PopulateUIVertex(ref vertex, index);
                        var ppos = vertex.position;
                        ppos.z = index;
                        vertex.position = ppos;
                        _quadVerts[j] = vertex;
                    }

                    //print(text[i] + "\t" + (int)text[i]);

                    if (isLeft)
                    {
//#if UNITY_2018_4_36
//                        if (_leftVerts.Count != 0 && _quadVerts[0].position.x > _leftVerts[_leftVerts.Count - 2].position.x + 2 * fontSize) // 右下顶点
//#else
                        if (_leftVerts.Count != 0 && !char.IsControl(text, i) && char.IsWhiteSpace(text, i))
//#endif
                        {
                            //print(text[i] + "\t" + (int)text[i]);
                            isLeft = false;
                            _rightVerts.AddRange(_quadVerts);
                        }
                        else
                            _leftVerts.AddRange(_quadVerts);
                    }
                    else
                        _rightVerts.AddRange(_quadVerts);
                }

                PartVertex(toFill);
            }

            m_DisableFontTextureRebuiltCallback = false;
        }

        private void PartVertex(VertexHelper toFill)
        {
            if (_leftVerts.Count == 0 && _rightVerts.Count == 0) return;

            //print($"left {_leftVerts.Count} right {_rightVerts.Count}");
            switch (alignment)
            {
                case TextAnchor.UpperLeft:
                case TextAnchor.MiddleLeft:
                case TextAnchor.LowerLeft:
                    {
                        if (_rightVerts.Count != 0)
                        {
                            var diff = rectTransform.rect.xMax - _rightVerts[_rightVerts.Count - 2].position.x;
                            for (int j = 0; j < _rightVerts.Count; j++)
                            {
                                var vert = _rightVerts[j];
                                var ppos = vert.position;
                                var index = (int)ppos.z;
                                vert.position = new Vector3(ppos.x + diff, ppos.y, 0);
                                toFill.SetUIVertex(vert, index);
                            }
                        }
                    }
                    break;
                default: throw new NotImplementedException("只支持左端对齐");
            }

            _leftVerts.Clear();
            _rightVerts.Clear();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (m_Shrink)
                horizontalOverflow = HorizontalWrapMode.Wrap;
            if (m_Gradient)
                supportRichText = false;
            if (m_AlignEnd)
            {
                switch (alignment)
                {
                    case TextAnchor.UpperCenter:
                    case TextAnchor.UpperRight:
                        alignment = TextAnchor.UpperLeft;
                        break;
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.MiddleRight:
                        alignment = TextAnchor.MiddleLeft;
                        break;
                    case TextAnchor.LowerCenter:
                    case TextAnchor.LowerRight:
                        alignment = TextAnchor.LowerLeft;
                        break;
                }
            }
        }
#endif
    }
}