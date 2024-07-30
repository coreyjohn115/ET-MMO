using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 对齐帮助类
    /// </summary>
    public static class AlignHelper
    {
        #region Methods
        /// <summary>
        /// 左对齐
        /// </summary>
        public static void AlignInHorizontalLeft()
        {
            float x = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.SetLocalX(x);
            }
        }

        /// <summary>
        /// 右对齐
        /// </summary>
        public static void AlignInHorizontalRight()
        {
            float x = Mathf.Max(Selection.gameObjects.Select(
                obj => obj.transform.localPosition.x + obj.AsTransform().sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                var pos = x - gameObject.AsTransform().sizeDelta.x;
                gameObject.transform.SetLocalX(pos);
            }
        }

        /// <summary>
        /// 上对齐
        /// </summary>
        public static void AlignInVerticalUp()
        {
            float y = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.SetLocalY(y);
            }
        }

        /// <summary>
        /// 下对齐
        /// </summary>
        public static void AlignInVerticalDown()
        {
            float y = Mathf.Min(Selection.gameObjects.Select(
                obj => obj.transform.localPosition.y - obj.AsTransform().sizeDelta.y).ToArray());

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                var pos = y + gameObject.AsTransform().sizeDelta.y;
                gameObject.transform.SetLocalY(pos);
            }
        }

        public static void UniformDistributionInHorizontal()
        {
//            EditorInputWin.ShowWin();
            var count = Selection.gameObjects.Length;
            var firstX = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());
            var lastX = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());
            var distance = (lastX - firstX) / (count - 1);
            var objects = Selection.gameObjects.ToList();
            objects.Sort((x, y) => (int)(x.transform.localPosition.x - y.transform.localPosition.x));
            for (int i = 0; i < count; i++)
            {
                objects[i].transform.SetLocalX(firstX + i * distance);
            }
        }

        public static void UniformDistributionInVertical()
        {
            var count = Selection.gameObjects.Length;
            var firstY = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            var lastY = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            var distance = (lastY - firstY) / (count - 1);
            var objects = Selection.gameObjects.ToList();
            objects.Sort((x, y) => (int)(x.transform.localPosition.y - y.transform.localPosition.y));
            for (int i = 0; i < count; i++)
            {
                objects[i].transform.SetLocalY(firstY + i * distance);
            }
        }

        public static void ResizeMax()
        {
            var height = Mathf.Max(Selection.gameObjects.Select(obj => obj.AsTransform().sizeDelta.y).ToArray());
            var width = Mathf.Max(Selection.gameObjects.Select(obj => obj.AsTransform().sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ((RectTransform)gameObject.transform).sizeDelta = new Vector2(width, height);
            }
        }

        public static void ResizeMin()
        {
            var height = Mathf.Min(Selection.gameObjects.Select(obj => obj.AsTransform().sizeDelta.y).ToArray());
            var width = Mathf.Min(Selection.gameObjects.Select(obj => obj.AsTransform().sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ((RectTransform)gameObject.transform).sizeDelta = new Vector2(width, height);
            }
        }
        #endregion
    }
}

