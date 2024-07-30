using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    /// <summary>
    /// 空的绘制，没有实际的显示，只有事件响应区域
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class EmptyGraphic : Graphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}
