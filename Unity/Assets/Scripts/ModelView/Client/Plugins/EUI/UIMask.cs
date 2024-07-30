using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 遮罩组件
    /// </summary>
    [ComponentOf(typeof (UIComponent))]
    public class UIMask: Entity, IAwake<bool>, IDestroy
    {
        /// <summary>
        /// 是否点击关闭界面
        /// </summary>
        public bool ClickHide;
        
        public RectTransform Mask;
    }
}