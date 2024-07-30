namespace ET.Client
{
    [EntitySystemOf(typeof (PhoneComponent))]
    public static partial class PhoneComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PhoneComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this PhoneComponent self)
        {
        }

        [EntitySystem]
        private static void LateUpdate(this PhoneComponent self)
        {
        }
    }
}