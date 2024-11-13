using Animancer;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (UnitGoComponent))]
    [FriendOf(typeof (UnitGoComponent))]
    public static partial class UnitGoComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this UnitGoComponent self)
        {
            UnityEngine.Object.Destroy(self.GameObject);
        }

        [EntitySystem]
        private static void Awake(this UnitGoComponent self, GameObject go)
        {
            self.GameObject = go;
            self.Transform = go.transform;
            self.collector = go.GetComponent<ReferenceCollector>();
        }

        public static Animator GetAnimator(this UnitGoComponent self)
        {
            return self.Get<Animator>("Animator");
        }

        public static AnimancerComponent GetAnimancer(this UnitGoComponent self)
        {
            return self.Get<AnimancerComponent>("Animancer");
        }

        /// <summary>
        /// 根据名称获取动画片段
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AnimationClip GetAnimationClip(this UnitGoComponent self, string name)
        {
            ReferenceCollector collect = self.Get<ReferenceCollector>("Clip");
            return collect.Get<AnimationClip>(name);
        }

        public static AudioSource GetAudioSource(this UnitGoComponent self)
        {
            return self.Get<AudioSource>("AudioSource");
        }

        /// <summary>
        /// 获取骨骼节点
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform GetBone(this UnitGoComponent self, string name)
        {
            return self.Get<Transform>(name);
        }

        private static T Get<T>(this UnitGoComponent self, string name) where T : UnityEngine.Object
        {
            return self.collector.Get<T>(name);
        }
    }
}