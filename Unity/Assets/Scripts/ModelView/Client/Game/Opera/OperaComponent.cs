﻿using System;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class OperaComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public Vector3 clickPoint;
    }
}