using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (UILoading))]
    public static partial class UILoadingSystem
    {
        public static void RegisterUIEvent(this UILoading self)
        {
        }

        public static void ShowWindow(this UILoading self, Entity contextData = null)
        {
            self.View.E_DescExtendText.SetActive(false);
            self.UpdateProcess(0);
        }

        public static void UpdateProcess(this UILoading self, float value)
        {
            self.View.E_SliderSlider.value = value;
            self.View.E_PctExtendText.text = value.ToString("0.00") + "%";
        }
    }
}