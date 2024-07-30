using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (UIPop))]
    public static partial class UIPopSystem
    {
        public static void RegisterUIEvent(this UIPop self)
        {
            self.Items = new[]
            {
                self.View.ESPopItem1, self.View.ESPopItem2, self.View.ESPopItem3, self.View.ESPopItem4, self.View.ESPopItem5,
                self.View.ESPopItem6, self.View.ESPopItem7, self.View.ESPopItem8, self.View.ESPopItem9, self.View.ESPopItem10
            };

            self.List = new Queue<ESPopItem>();
            foreach (var value in self.Items)
            {
                self.List.Enqueue(value);
            }

            self.InitPos = self.Items[0].GetPosition();
            self.ItemSize = new Vector3(0, self.Items[0].GetHeight() + self.ItemSpace, 0);
        }

        public static void ShowWindow(this UIPop self, Entity contextData = null)
        {
        }

        public static void PopMsg(this UIPop self, string msg, Color? color)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }

            ESPopItem item = self.List.Dequeue();
            item.SetMsg(msg, color);
            item.Replay(self.InitPos);
            var arr = self.List.ToArray();
            int j = 0;
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                j++;
                ESPopItem it = arr[i];
                if (it.IsAlive())
                {
                    var pos = self.InitPos + self.ItemSize * j;
                    it.MoveTo(pos);
                }
                else
                {
                    break;
                }
            }

            self.List.Enqueue(item);
        }
    }
}