using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace ET.Client
{
    [AddComponentMenu("UI/ExtendImage")]
    public class ExtendImage : Image
    {
        [SerializeField]
        private bool m_Grey;

        [SerializeField]
        private bool m_CircleMask;

        //Filled模式使用SlicedClipMode
        [SerializeField]
        private bool m_SlicedClipMode = false;

        [SerializeField]
        private RectTransform m_HandleRect;

        private static Material m_GreyMaterial; // 灰色材质

        private static Material GreyMaterial
        {
            get
            {
                //if (m_GreyMaterial == null)
                //    m_GreyMaterial = Core.res.LoadGlobal<Material>("System/Mats/UIGray");
                return m_GreyMaterial;
            }
        }

        public bool Grey
        {
            get => m_Grey; 
            set
            {
                if (m_Grey == value)
                    return;

                m_Grey = value;
                material = m_Grey ? GreyMaterial : null;
                SetMaterialDirty();
            }
        }

        public bool CircleMask
        {
            get => m_CircleMask; 
            set
            {
                if (m_CircleMask == value)
                    return;

                m_CircleMask = value;
                SetMaterialDirty();
            }
        }

        /// <summary>
        ///  Image的Type为Simple时,镜像可选模式
        /// </summary>
        public enum Mirror
        {
            /// <summary>
            /// 不镜像
            /// </summary>
            None

          , /// <summary>
            /// 水平镜像
            /// </summary>
            Horizontal

          , /// <summary>
            /// 竖直镜像
            /// </summary>
            Vertical
        }

        [SerializeField]
        private Mirror m_Mirror = Mirror.None; // 镜像模式

        #region 重载部分

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var activeSprite = overrideSprite ?? sprite;
            switch (type)
            {
                // 为了简洁,暂时只有simple方式且不使用精灵网格的才可以镜像
                case Type.Simple when !useSpriteMesh && activeSprite != null && m_Mirror != Mirror.None:
                    GenerateSimpleSprite(vh);
                    break;
                case Type.Simple when !useSpriteMesh && activeSprite != null && m_CircleMask:
                    CircleMaskSprite(vh);
                    break;
                case Type.Filled when m_SlicedClipMode && (fillMethod == FillMethod.Horizontal || fillMethod == FillMethod.Vertical):
                    GenerateSlicedSprite(vh);
                    break;
                default:
#if UNITY_EDITOR && EXTEND_UI_DEBUG
                    print("Base OnPopulateMesh");
#endif
                    base.OnPopulateMesh(vh);
                    break;
            }
            //UIVertex vert = new UIVertex();
            //for (int i = 0; i < vh.currentVertCount; i++)
            //{
            //    vh.PopulateUIVertex(ref vert, i);
            //    vert.uv1.x = (i >> 1);
            //    vert.uv1.y = ((i >> 1) ^ (i & 1));
            //    vert.uv2.x = m_Grey ? 1 : 0;
            //    vert.uv2.y = m_CircleMask ? 1 : 0;
            //    vh.SetUIVertex(vert, i);
            //}
            //if (m_GreyMaterial && (m_CircleMask || m_Grey))
            //{
            //    var uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
            //    m_GreyMaterial.SetVector("_UvRect", uv);
            //    m_GreyMaterial.SetFloat("_ExtandUI", 1);
            //}
            if (type == Type.Filled)
            {
                FillHandle();
            }
        }

        public override void SetNativeSize()
        {
            var activeSprite = overrideSprite ?? sprite;
            if (activeSprite != null)
            {
                var w = sprite.rect.width / pixelsPerUnit;
                var h = sprite.rect.height / pixelsPerUnit;
                if (m_Mirror == Mirror.Horizontal && !useSpriteMesh)
                    w *= 2;
                else if (m_Mirror == Mirror.Vertical && !useSpriteMesh)
                    h *= 2;

                rectTransform.anchorMax = rectTransform.anchorMin;
                rectTransform.sizeDelta = new Vector2(w, h);
                SetAllDirty();
            }
        }

        private void GenerateSimpleSprite(VertexHelper vh)
        {
            var lPreserveAspect = preserveAspect;
            var activeSprite    = overrideSprite ?? sprite;

            var v       = GetDrawingDimensions(lPreserveAspect);
            var v1      = v;
            var uv      = activeSprite != null ? DataUtility.GetOuterUV(activeSprite) : Vector4.zero;
            var color32 = color;
            vh.Clear();

            // 参考：https://github.com/codingriver/UnityProjectTest/blob/master/MirrorImage/Assets/Script/MirrorImage.cs
            if (m_Mirror == Mirror.Horizontal)
            {
                v.z = (v.x + v.z) / 2;
                vh.AddVert(new Vector3(v.x,  v.y),  color32, new Vector2(uv.x, uv.y));
                vh.AddVert(new Vector3(v.x,  v.w),  color32, new Vector2(uv.x, uv.w));
                vh.AddVert(new Vector3(v.z,  v.w),  color32, new Vector2(uv.z, uv.w));
                vh.AddVert(new Vector3(v.z,  v.y),  color32, new Vector2(uv.z, uv.y));
                vh.AddVert(new Vector3(v1.z, v1.y), color32, new Vector2(uv.x, uv.y));
                vh.AddVert(new Vector3(v1.z, v1.w), color32, new Vector2(uv.x, uv.w));

                vh.AddTriangle(0, 1, 2);
                vh.AddTriangle(2, 3, 0);
                vh.AddTriangle(3, 2, 5);
                vh.AddTriangle(5, 4, 3);
            }
            else if (m_Mirror == Mirror.Vertical)
            {
                v.w = (v.w + v.y) / 2;
                vh.AddVert(new Vector3(v.x,  v.y),  color32, new Vector2(uv.x, uv.y));
                vh.AddVert(new Vector3(v.x,  v.w),  color32, new Vector2(uv.x, uv.w));
                vh.AddVert(new Vector3(v.z,  v.w),  color32, new Vector2(uv.z, uv.w));
                vh.AddVert(new Vector3(v.z,  v.y),  color32, new Vector2(uv.z, uv.y));
                vh.AddVert(new Vector3(v1.x, v1.w), color32, new Vector2(uv.x, uv.y));
                vh.AddVert(new Vector3(v1.z, v1.w), color32, new Vector2(uv.z, uv.y));

                vh.AddTriangle(0, 1, 2);
                vh.AddTriangle(2, 3, 0);
                vh.AddTriangle(1, 4, 5);
                vh.AddTriangle(5, 2, 1);
            }
        }

        private void CircleMaskSprite(VertexHelper vh)
        {
            vh.Clear();

            float tw = rectTransform.rect.width;
            float th = rectTransform.rect.height;
            float outerRadius = 0.5f * tw;
            float vertexCenterX = (0.5f - rectTransform.pivot.x) * tw;
            float vertexCenterY = (0.5f - rectTransform.pivot.y) * th;

            Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
            float uvCenterX = (uv.x + uv.z) * 0.5f;
            float uvCenterY = (uv.y + uv.w) * 0.5f;
            float uvRadius = (uv.z - uv.x) * 0.5f;

            float degreeDelta = (float)(2 * Mathf.PI / 30);
            int curSegements = 30;

            float curDegree = 0;
            UIVertex uiVertex;
            int verticeCount;
            int triangleCount;
            Vector2 curVertice;
            Vector2 curUV;
            curVertice = new Vector2(vertexCenterX, vertexCenterY);
            verticeCount = curSegements + 1;
            uiVertex = new UIVertex();
            uiVertex.color = color;
            uiVertex.position = curVertice;
            uiVertex.uv0 = new Vector2(uvCenterX, uvCenterY);
            vh.AddVert(uiVertex);

            for (int i = 1; i < verticeCount; i++)
            {
                float cosA = Mathf.Cos(curDegree);
                float sinA = Mathf.Sin(curDegree);
                curVertice = new Vector2(cosA * outerRadius, sinA * outerRadius);
                curUV = new Vector2(cosA * uvRadius, sinA * uvRadius);
                curDegree += degreeDelta;

                uiVertex = new UIVertex();
                uiVertex.color = color;
                uiVertex.position.x = vertexCenterX + curVertice.x;
                uiVertex.position.y = vertexCenterY + curVertice.y;
                uiVertex.uv0 = new Vector2(curUV.x + uvCenterX, curUV.y + uvCenterY);
                vh.AddVert(uiVertex);
            }
            triangleCount = curSegements * 3;
            for (int i = 0, vIdx = 1; i < triangleCount - 3; i += 3, vIdx++)
            {
                vh.AddTriangle(vIdx, 0, vIdx + 1);
            }
            vh.AddTriangle(verticeCount - 1, 0, 1);
        }

        static readonly Vector2[] s_VertScratch = new Vector2[4];
        //static readonly Vector2[] s_
        static readonly Vector2[] s_UVScratch = new Vector2[4];

        private void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
        {
            Vector4 v = GetDrawingDimensions(lPreserveAspect);
            var activeSprite = overrideSprite ?? sprite;
            var uv = (activeSprite != null) ? UnityEngine.Sprites.DataUtility.GetOuterUV(activeSprite) : Vector4.zero;

            var color32 = color;
            vh.Clear();
            vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(uv.x, uv.y));
            vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(uv.x, uv.w));
            vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(uv.z, uv.w));
            vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(uv.z, uv.y));

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        private void GenerateSlicedSprite(VertexHelper toFill)
        {
            if (!hasBorder)
            {
                GenerateSimpleSprite(toFill, false);
                return;
            }

            var activeSprite = overrideSprite ?? sprite;

            Vector4 outer, inner, padding, border;

            if (activeSprite != null)
            {
                outer = UnityEngine.Sprites.DataUtility.GetOuterUV(activeSprite);
                inner = UnityEngine.Sprites.DataUtility.GetInnerUV(activeSprite);
                padding = UnityEngine.Sprites.DataUtility.GetPadding(activeSprite);
                border = activeSprite.border;
            }
            else
            {
                outer = Vector4.zero;
                inner = Vector4.zero;
                padding = Vector4.zero;
                border = Vector4.zero;
            }
            Rect rect = GetPixelAdjustedRect();
            Vector4 adjustedBorders = GetAdjustedBorders(border / pixelsPerUnit, rect);
            padding = padding / pixelsPerUnit;

            s_VertScratch[0] = new Vector2(padding.x, padding.y);
            s_VertScratch[3] = new Vector2(rect.width - padding.z, rect.height - padding.w);

            s_VertScratch[1].x = adjustedBorders.x;
            s_VertScratch[1].y = adjustedBorders.y;

            s_VertScratch[2].x = rect.width - adjustedBorders.z;
            s_VertScratch[2].y = rect.height - adjustedBorders.w;

            for (int i = 0; i < 4; ++i)
            {
                s_VertScratch[i].x += rect.x;
                s_VertScratch[i].y += rect.y;
            }

            s_UVScratch[0] = new Vector2(outer.x, outer.y);
            s_UVScratch[1] = new Vector2(inner.x, inner.y);
            s_UVScratch[2] = new Vector2(inner.z, inner.w);
            s_UVScratch[3] = new Vector2(outer.z, outer.w);

            float xLength = s_VertScratch[3].x - s_VertScratch[0].x;
            float yLength = s_VertScratch[3].y - s_VertScratch[0].y;
            float len1XRatio = (s_VertScratch[1].x - s_VertScratch[0].x) / xLength;
            float len1YRatio = (s_VertScratch[1].y - s_VertScratch[0].y) / yLength;
            float len2XRatio = (s_VertScratch[2].x - s_VertScratch[1].x) / xLength;
            float len2YRatio = (s_VertScratch[2].y - s_VertScratch[1].y) / yLength;
            float len3XRatio = (s_VertScratch[3].x - s_VertScratch[2].x) / xLength;
            float len3YRatio = (s_VertScratch[3].y - s_VertScratch[2].y) / yLength;
            int xLen = 3, yLen = 3;
            if (fillMethod == FillMethod.Horizontal)
            {
                if (fillAmount >= (len1XRatio + len2XRatio))
                {
                    float ratio = 1 - (fillAmount - (len1XRatio + len2XRatio)) / len3XRatio;
                    s_VertScratch[3].x = s_VertScratch[3].x - (s_VertScratch[3].x - s_VertScratch[2].x) * ratio;
                    s_UVScratch[3].x = s_UVScratch[3].x - (s_UVScratch[3].x - s_UVScratch[2].x) * ratio;
                }
                else if (fillAmount >= len1XRatio)
                {
                    xLen = 2;
                    float ratio = 1 - (fillAmount - len1XRatio) / len2XRatio;
                    s_VertScratch[2].x = s_VertScratch[2].x - (s_VertScratch[2].x - s_VertScratch[1].x) * ratio;
                }
                else
                {
                    xLen = 1;
                    float ratio = 1 - fillAmount / len1XRatio;
                    s_VertScratch[1].x = s_VertScratch[1].x - (s_VertScratch[1].x - s_VertScratch[0].x) * ratio;
                    s_UVScratch[1].x = s_UVScratch[1].x - (s_UVScratch[1].x - s_UVScratch[0].x) * ratio;
                }
            }
            else if (fillMethod == FillMethod.Vertical)
            {
                if (fillAmount >= (len1YRatio + len2YRatio))
                {
                    float ratio = 1 - (fillAmount - (len1YRatio + len2YRatio)) / len3YRatio;
                    s_VertScratch[3].y = s_VertScratch[3].y - (s_VertScratch[3].y - s_VertScratch[2].y) * ratio;
                    s_UVScratch[3].y = s_UVScratch[3].y - (s_UVScratch[3].y - s_UVScratch[2].y) * ratio;
                }
                else if (fillAmount >= len1YRatio)
                {
                    yLen = 2;
                    float ratio = 1 - (fillAmount - len1YRatio) / len2YRatio;
                    s_VertScratch[2].y = s_VertScratch[2].y - (s_VertScratch[2].y - s_VertScratch[1].y) * ratio;
                }
                else
                {
                    yLen = 1;
                    float ratio = 1 - fillAmount / len1YRatio;
                    s_VertScratch[1].y = s_VertScratch[1].y - (s_VertScratch[1].y - s_VertScratch[0].y) * ratio;
                    s_UVScratch[1].y = s_UVScratch[1].y - (s_UVScratch[1].y - s_UVScratch[0].y) * ratio;
                }
            }

            toFill.Clear();

            for (int x = 0; x < xLen; ++x)
            {
                int x2 = x + 1;

                for (int y = 0; y < yLen; ++y)
                {
                    if (!fillCenter && x == 1 && y == 1)
                        continue;

                    int y2 = y + 1;


                    AddQuad(toFill,
                        new Vector2(s_VertScratch[x].x, s_VertScratch[y].y),
                        new Vector2(s_VertScratch[x2].x, s_VertScratch[y2].y),
                        color,
                        new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
                        new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));
                }
            }
        }

        static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
        {
            Rect originalRect = rectTransform.rect;

            for (int axis = 0; axis <= 1; axis++)
            {
                float borderScaleRatio;

                // The adjusted rect (adjusted for pixel correctness)
                // may be slightly larger than the original rect.
                // Adjust the border to match the adjustedRect to avoid
                // small gaps between borders (case 833201).
                if (originalRect.size[axis] != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / originalRect.size[axis];
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }

                // If the rect is smaller than the combined borders, then there's not room for the borders at their normal size.
                // In order to avoid artefacts with overlapping borders, we scale the borders down to fit.
                float combinedBorders = border[axis] + border[axis + 2];
                if (adjustedRect.size[axis] < combinedBorders && combinedBorders != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / combinedBorders;
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
            }
            return border;
        }

        private void FillHandle()
        {
            if (m_HandleRect == null) return;
            if(fillMethod == FillMethod.Horizontal || fillMethod == FillMethod.Vertical)
            {
                m_HandleRect.anchorMin = new Vector2(fillAmount, 0.0f);
                m_HandleRect.anchorMax = new Vector2(fillAmount, 1.0f);        
            }
            else if (fillMethod == FillMethod.Radial90)
            {
                m_HandleRect.localEulerAngles = new Vector3(0, 0, -fillAmount * 90);
            }
            else if (fillMethod == FillMethod.Radial180)
            {
                m_HandleRect.localEulerAngles = new Vector3(0, 0, -fillAmount * 180);
            }
            else if(fillMethod == FillMethod.Radial360)
            {
                m_HandleRect.localEulerAngles = new Vector3(0, 0, -fillAmount * 360);
            }
        }

#if UNITY_EDITOR
        //protected override void OnValidate()
        //{
        //    material = m_Grey ? GreyMaterial : null;
        //    if (type != Type.Simple || useSpriteMesh)
        //        m_Mirror = Mirror.None;
        //    base.OnValidate();
        //}
#endif
        // TODO 
        // public void Localize(Sprite localSprite)
        // {
        //    sprite = localSprite;
        // }

        #endregion

        #region 需要的Image源码

        /// Image's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
        private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
        {
            var activeSprite = overrideSprite ?? sprite;
            var padding      = activeSprite == null ? Vector4.zero : DataUtility.GetPadding(activeSprite);
            var size         = activeSprite == null ? Vector2.zero : new Vector2(activeSprite.rect.width, activeSprite.rect.height);

            var r = GetPixelAdjustedRect();
            // Debug.Log(string.Format("r:{2}, size:{0}, padding:{1}", size, padding, r));

            var spriteW = Mathf.RoundToInt(size.x);
            var spriteH = Mathf.RoundToInt(size.y);

            var v = new Vector4(
                                padding.x / spriteW
                              , padding.y / spriteH
                              , (spriteW - padding.z) / spriteW
                              , (spriteH - padding.w) / spriteH);

            if (shouldPreserveAspect && size.sqrMagnitude > 0.0f) PreserveSpriteAspectRatio(ref r, size);

            v = new Vector4(
                            r.x + r.width * v.x
                          , r.y + r.height * v.y
                          , r.x + r.width * v.z
                          , r.y + r.height * v.w
                           );

            return v;
        }

        private void PreserveSpriteAspectRatio(ref Rect rect, Vector2 spriteSize)
        {
            var spriteRatio = spriteSize.x / spriteSize.y;
            var rectRatio   = rect.width / rect.height;

            if (spriteRatio > rectRatio)
            {
                var oldHeight = rect.height;
                rect.height =  rect.width * (1.0f / spriteRatio);
                rect.y      += (oldHeight - rect.height) * rectTransform.pivot.y;
            }
            else
            {
                var oldWidth = rect.width;
                rect.width =  rect.height * spriteRatio;
                rect.x     += (oldWidth - rect.width) * rectTransform.pivot.x;
            }
        }

        #endregion
    }
}