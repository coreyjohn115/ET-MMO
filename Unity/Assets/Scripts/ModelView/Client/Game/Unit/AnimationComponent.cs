using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class AnimationComponent: Entity, IAwake, IDestroy
    {
        public Animation animation;
        
        public long timer;
        public long battleChangeTime;

        /// <summary>
        /// 姿势列表
        /// </summary>
        public List<string> postureList = new();

        public int lastAnimLayer = 0;
        public string lastAnimName = null;
        public int lastAnimStateFrame = 0;
        public AnimationState lastAnimState = null;
        
        public const string Idle = "Idle";
        public const string Run = "Run";
        public const string Die = "Die";
        public const string Postore = "Postore";
    }
}