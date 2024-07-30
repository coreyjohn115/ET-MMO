namespace ET.Client
{
    [EntitySystemOf(typeof (UIComComponent))]
    public static partial class UIComComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIComComponent self)
        {
        }

        public static void Show(this UIComComponent self)
        {
            UIIComSingleton.Instance.Show(self.Parent.GetType().Name, self);
        }
    }
}