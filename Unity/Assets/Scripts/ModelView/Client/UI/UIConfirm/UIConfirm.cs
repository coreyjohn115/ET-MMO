using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public enum UIConfirmType
    {
        None,
        Ok,
        Cancel,
    }

    public enum UIConfirmStyle
    {
        Blue,
        Yellow,
        Red,
    }

    public struct ConfirmBtn
    {
        public UIConfirmType ConfirmType;
        public UIConfirmStyle ConfirmStyle;
        public int TextId;

        public ConfirmBtn(UIConfirmType confirmType, UIConfirmStyle style = UIConfirmStyle.Blue, int textId = 0)
        {
            ConfirmType = confirmType;
            ConfirmStyle = style;
            this.TextId = textId;
            if (this.TextId > 0)
            {
                return;
            }

            switch (this.ConfirmType)
            {
                case UIConfirmType.None:
                    this.TextId = 106002;
                    break;
                case UIConfirmType.Ok:
                    this.TextId = 106001;
                    break;
                case UIConfirmType.Cancel:
                    this.TextId = 106002;
                    break;
            }
        }
    }

    [ComponentOf(typeof (UIBaseWindow))]
    public class UIConfirm: Entity, IAwake, IUILogic
    {
        public UIConfirmType confirmType;
        public UIConfirmExtra extra;

        public List<GameObject> btnGoList = new();

        public Dictionary<ConfirmTipType, UIConfirmType> nTipDict = new();

        public UIConfirmViewComponent View
        {
            get => GetParent<UIBaseWindow>().GetComponent<UIConfirmViewComponent>();
        }
    }
}