using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (UIBaseWindow))]
    public class UIPop: Entity, IAwake, IUILogic
    {
        public UIPopViewComponent View
        {
            get => GetParent<UIBaseWindow>().GetComponent<UIPopViewComponent>();
        }

        public ESPopItem[] Items;
        public Queue<ESPopItem> List;

        public float ItemSpace = 5;
        public Vector3 InitPos;
        public Vector3 ItemSize;
    }
}