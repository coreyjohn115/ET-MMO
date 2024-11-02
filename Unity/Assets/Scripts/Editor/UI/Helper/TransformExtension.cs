using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    public static class TransformExtension
    {
        #region Public Methods
        public static RectTransform AsTransform(this GameObject gameObject)
        {
            if (gameObject.transform is RectTransform transform)
            {
                return transform;
            }

            return null;
        }

        public static RectTransform AsTransform(this Transform transform)
        {
            if (transform is RectTransform trans)
            {
                return trans;
            }

            return null;
        }

        /// <summary>
        /// 设置世界X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Transform SetX(this Transform transform, float x)
        {
            var pos = transform.position;
            pos.x = x;
            transform.position = pos;

            return transform;
        }

        /// <summary>
        /// 设置局部X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Transform SetLocalX(this Transform transform, float x)
        {
            var pos = transform.localPosition;
            pos.x = x;
            transform.localPosition = pos;

            return transform;
        }

        /// <summary>
        /// 设置世界X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Transform SetY(this Transform transform, float y)
        {
            var pos = transform.position;
            pos.y = y;
            transform.position = pos;

            return transform;
        }

        /// <summary>
        /// 设置局部X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Transform SetLocalY(this Transform transform, float y)
        {
            var pos = transform.localPosition;
            pos.y = y;
            transform.localPosition = pos;

            return transform;
        }

        /// <summary>
        /// 设置世界X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Transform SetZ(this Transform transform, float z)
        {
            var pos = transform.position;
            pos.z = z;
            transform.position = pos;

            return transform;
        }

        /// <summary>
        /// 设置局部X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Transform SetLocalZ(this Transform transform, float z)
        {
            var pos = transform.localPosition;
            pos.z = z;
            transform.localPosition = pos;

            return transform;
        }

        /// <summary>
        /// 设置世界X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static RectTransform SetX(this RectTransform transform, float x)
        {
            var pos = transform.position;
            pos.x = x;
            transform.position = pos;

            return transform;
        }

        /// <summary>
        /// 设置局部X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static RectTransform SetLocalX(this RectTransform transform, float x)
        {
            var pos = transform.localPosition;
            pos.x = x;
            transform.localPosition = pos;

            return transform;
        }

        /// <summary>
        /// 设置世界X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectTransform SetY(this RectTransform transform, float y)
        {
            var pos = transform.position;
            pos.y = y;
            transform.position = pos;

            return transform;
        }

        /// <summary>
        /// 设置局部X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectTransform SetLocalY(this RectTransform transform, float y)
        {
            var pos = transform.localPosition;
            pos.y = y;
            transform.localPosition = pos;

            return transform;
        }

        /// <summary>
        /// 设置世界X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static RectTransform SetZ(this RectTransform transform, float z)
        {
            var pos = transform.position;
            pos.z = z;
            transform.position = pos;

            return transform;
        }

        /// <summary>
        /// 设置局部X坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static RectTransform SetLocalZ(this RectTransform transform, float z)
        {
            var pos = transform.localPosition;
            pos.z = z;
            transform.localPosition = pos;

            return transform;
        }

        /// <summary>
        /// 设置ScaleX
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static RectTransform SetScaleX(this RectTransform transform, float x)
        {
            var scale = transform.localScale;
            scale.x = x;
            transform.localScale = scale;

            return transform;
        }

        /// <summary>
        /// 设置ScaleY
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectTransform SetScaleY(this RectTransform transform, float y)
        {
            var scale = transform.localScale;
            scale.y = y;
            transform.localScale = scale;

            return transform;
        }

        /// <summary>
        /// 设置ScaleZ
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static RectTransform SetScaleZ(this RectTransform transform, float z)
        {
            var scale = transform.localScale;
            scale.z = z;
            transform.localScale = scale;

            return transform;
        }

        /// <summary>
        /// 设置世界X坐标
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static RectTransform SetX<T>(this T graph, float x) where T : Graphic
        {
            var pos = graph.rectTransform.position;
            pos.x = x;
            graph.rectTransform.position = pos;

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置局部X坐标
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static RectTransform SetLocalX<T>(this T graph, float x) where T : Graphic
        {
            var pos = graph.rectTransform.localPosition;
            pos.x = x;
            graph.rectTransform.localPosition = pos;

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置世界X坐标
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectTransform SetY<T>(this T graph, float y) where T : Graphic
        {
            var pos = graph.rectTransform.position;
            pos.y = y;
            graph.rectTransform.position = pos;

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置局部X坐标
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectTransform SetLocalY<T>(this T graph, float y) where T : Graphic
        {
            var pos = graph.rectTransform.localPosition;
            pos.y = y;
            graph.rectTransform.localPosition = pos;

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置世界X坐标
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static RectTransform SetZ<T>(this T graph, float z) where T : Graphic
        {
            var pos = graph.rectTransform.position;
            pos.z = z;
            graph.rectTransform.position = pos;

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置局部X坐标
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static RectTransform SetLocalZ<T>(this T graph, float z) where T : Graphic
        {
            var pos = graph.rectTransform.localPosition;
            pos.z = z;
            graph.rectTransform.localPosition = pos;

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置ScaleX
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static RectTransform SetScaleX<T>(this T graph, float x) where T : Graphic
        {
            var scale = graph.rectTransform.localScale;
            scale.x = x;
            graph.rectTransform.localScale = scale;

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置ScaleY
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectTransform SetScaleY<T>(this T graph, float y) where T : Graphic
        {
            var scale = graph.rectTransform.localScale;
            scale.y = y;
            graph.rectTransform.localScale = scale;

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置ScaleZ
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static RectTransform SetScaleZ<T>(this T graph, float z) where T : Graphic
        {
            var scale = graph.rectTransform.localScale;
            scale.z = z;
            graph.rectTransform.localScale = scale;

            return graph.rectTransform;
        }

        public static RectTransform SetAnchorX(this RectTransform transform, float x)
        {
            var anchorPos = transform.anchoredPosition;
            anchorPos.x = x;
            transform.anchoredPosition = anchorPos;

            return transform;
        }

        public static RectTransform SetAnchorY(this RectTransform transform, float y)
        {
            var anchorPos = transform.anchoredPosition;
            anchorPos.y = y;
            transform.anchoredPosition = anchorPos;

            return transform;
        }

        public static RectTransform SetPivotX(this RectTransform transform, float x)
        {
            var pivot = transform.pivot;
            pivot.x = x;
            transform.pivot = pivot;

            return transform;
        }

        public static RectTransform SetPivotY(this RectTransform transform, float y)
        {
            var pivot = transform.pivot;
            pivot.y = y;
            transform.pivot = pivot;

            return transform;
        }

        public static RectTransform SetAnchorX<T>(this T graph, float x) where T : Graphic
        {
            var anchorPos = graph.rectTransform.anchoredPosition;
            anchorPos.x = x;
            graph.rectTransform.anchoredPosition = anchorPos;

            return graph.rectTransform;
        }

        public static RectTransform SetAnchorY<T>(this T graph, float y) where T : Graphic
        {
            var anchorPos = graph.rectTransform.anchoredPosition;
            anchorPos.y = y;
            graph.rectTransform.anchoredPosition = anchorPos;

            return graph.rectTransform;
        }

        public static RectTransform SetPivotX<T>(this T graph, float x) where T : Graphic
        {
            var pivot = graph.rectTransform.pivot;
            pivot.x = x;
            graph.rectTransform.pivot = pivot;

            return graph.rectTransform;
        }

        public static RectTransform SetPivotY<T>(this T graph, float y) where T : Graphic
        {
            var pivot = graph.rectTransform.pivot;
            pivot.y = y;
            graph.rectTransform.pivot = pivot;

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置宽度
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static RectTransform SetWidth(this RectTransform transform, float width)
        {
           transform.SetSizeWithCurrentAnchors(
               RectTransform.Axis.Horizontal, width);

            return transform;
        }

        /// <summary>
        /// 设置高度
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static RectTransform SetHeight(this RectTransform transform, float height)
        {
            transform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical, height);

            return transform;
        }

        /// <summary>
        /// 设置宽度
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static RectTransform SetWidth<T>(this T graph, float width)
            where T : Graphic
        {
            graph.rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal, width);

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置高度
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static RectTransform SetHeight<T>(this T graph, float height) 
            where T : Graphic
        {
            graph.rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical, height);

            return graph.rectTransform;
        }

        /// <summary>
        /// 设置宽度
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static float GetWidth(this RectTransform transform)
        {
            return transform.sizeDelta.x;
        }

        /// <summary>
        /// 设置高度
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static float GetHeight(this RectTransform transform)
        {
            return transform.sizeDelta.y;
        }

        /// <summary>
        /// 得到宽度
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static float GetWidth<T>(this T graph)
            where T : Graphic
        {
            return graph.rectTransform.sizeDelta.x;
        }

        /// <summary>
        /// 得到高度
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static float GetHeight<T>(this T graph)
            where T : Graphic
        {
            return graph.rectTransform.sizeDelta.y;
        }

        public static float GetX<T>(this T graphc) where T : Graphic
        {
            return graphc.rectTransform.position.x;
        }

        public static float GetY<T>(this T graphc) where T : Graphic
        {
            return graphc.rectTransform.position.y;
        }

        public static float GetZ<T>(this T graphc) where T : Graphic
        {
            return graphc.rectTransform.position.z;
        }

        public static float GetLocalX<T>(this T graphc) where T : Graphic
        {
            return graphc.rectTransform.localPosition.x;
        }

        public static float GetLocalY<T>(this T graphc) where T : Graphic
        {
            return graphc.rectTransform.localPosition.y;
        }

        public static float GetLocalZ<T>(this T graphc) where T : Graphic
        {
            return graphc.rectTransform.localPosition.z;
        }

        public static float GetAnchorX<T>(this T graphc) where T : Graphic
        {
            return graphc.rectTransform.anchoredPosition.x;
        }

        public static float GetAnchorY<T>(this T graphc) where T : Graphic
        {
            return graphc.rectTransform.anchoredPosition.y;
        }

        /// <summary>
        /// 获取中心点坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Vector2 CenterPosition(this RectTransform transform)
        {
            var pos = transform.anchoredPosition;

            return pos + transform.sizeDelta * 0.5f;
        }

        /// <summary>
        /// 获取中心点坐标
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static Vector2 CenterPosition<T>(this T graph) where T : Graphic
        {
            var pos = graph.rectTransform.anchoredPosition;

            return pos + graph.rectTransform.sizeDelta * 0.5f;
        }
        #endregion
    }
}
