using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// 传递布局事件,让父物体的父物体之类重新布局
    /// </summary>
    public class LayoutTransmitter: UIBehaviour
    {
        [SerializeField]
        private RectTransform target;

        [SerializeField]
        private bool delayOneFrame = true;

        protected override void OnRectTransformDimensionsChange()
        {
            StopAllCoroutines();
            StartCoroutine(DelayCO());
        }

        private IEnumerator DelayCO()
        {
            if (this.delayOneFrame)
            {
                yield return null;
            }

            if (this.target)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(this.target);
            }
        }
    }
}