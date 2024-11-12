using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// 全局挂载脚本调试，可视化查看哪些UI接收点击事件
    /// </summary>
    public class DebugUILine: MonoBehaviour
    {
        #region Internal Methods

        private void OnDrawGizmos()
        {
            if (!enable)
            {
                return;
            }

            var lastColor = Gizmos.color;
            Gizmos.color = color;
            foreach (MaskableGraphic g in FindObjectsByType<MaskableGraphic>(FindObjectsSortMode.None))
            {
                if (g.raycastTarget)
                {
                    RectTransform rectTransform = g.transform as RectTransform;
                    rectTransform.GetWorldCorners(fourCorners);

                    for (int i = 0; i < 4; i++)
                    {
                        Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
                    }
                }
            }

            Gizmos.color = lastColor;
        }

        #endregion

        #region Internal Fields

        [SerializeField]
        private bool enable = true;

        [SerializeField]
        private Color color = Color.blue;

        static Vector3[] fourCorners = new Vector3[4];

        #endregion
    }
}