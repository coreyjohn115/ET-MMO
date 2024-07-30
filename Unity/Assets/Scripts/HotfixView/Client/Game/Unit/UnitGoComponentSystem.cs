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

        private static T Get<T>(this UnitGoComponent self, string name) where T : UnityEngine.Object
        {
            return self.collector.Get<T>(name);
        }

        public static Animator GetAnimator(this UnitGoComponent self)
        {
            return self.Get<Animator>("Animator");
        }

        public static Animation GetAnimation(this UnitGoComponent self)
        {
            return self.Get<Animation>("Animation");
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
    }
}