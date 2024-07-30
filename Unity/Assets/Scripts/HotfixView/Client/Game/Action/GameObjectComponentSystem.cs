namespace ET.Client
{
    [EntitySystemOf(typeof (GameObjectComponent))]
    [FriendOf(typeof (GameObjectComponent))]
    public static partial class GameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GameObjectComponent self, UnityEngine.GameObject prefab)
        {
            var go = UnityEngine.Object.Instantiate(prefab);
            self.GameObject = go;
            self.Transform = go.transform;
        }

        [EntitySystem]
        private static void Destroy(this GameObjectComponent self)
        {
            UnityEngine.Object.Destroy(self.GameObject);
            self.GameObject = default;
            self.Transform = default;
        }
    }
}