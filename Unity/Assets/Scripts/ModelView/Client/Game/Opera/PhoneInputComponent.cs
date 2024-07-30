using System;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class PhoneComponent: Entity, IAwake, IUpdate, ILateUpdate
    {
        public bool moving;
    }
}