using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 不再提示类型
    /// </summary>
    public enum ConfirmTipType
    {
        None = 0,
        Test,
    }

    public struct UIConfirmExtra
    {
        /// <summary>
        /// 点击空白是否关闭界面
        /// </summary>
        public bool EmptyClose;

        /// <summary>
        /// 本次登录不再提示类型 默认不显示
        /// </summary>
        public ConfirmTipType TipType;
    }

    [ComponentOf(typeof (Scene))]
    public class UIConfirmData: Entity, IAwake
    {
        public string Title { get; set; }

        public string Desc { get; set; }

        public UIConfirmExtra Extra { get; set; }

        public List<ConfirmBtn> BtnList { get; } = new();
    }
}