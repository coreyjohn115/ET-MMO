namespace ET
{
    [EntitySystemOf(typeof (Unit))]
    public static partial class UnitSystem
    {
        [EntitySystem]
        private static void Awake(this Unit self, int configId)
        {
            self.ConfigId = configId;
        }

        public static UnitConfig Config(this Unit self)
        {
            return UnitConfigCategory.Instance.Get(self.ConfigId);
        }

        public static UnitType Type(this Unit self)
        {
            return (UnitType)self.Config().Type;
        }

        public static void FromProto(this UnitHead self, HeadProto proto)
        {
            self.HeadIcon = proto.HeadIcon;
            self.ChatFrame = proto.ChatFrame;
            self.ChatBubble = proto.ChatBubble;
        }

        public static HeadProto ToProto(this UnitHead self)
        {
            HeadProto proto = HeadProto.Create();
            proto.HeadIcon = self.HeadIcon;
            proto.ChatFrame = self.ChatFrame;
            proto.ChatBubble = self.ChatBubble;
            return proto;
        }
    }
}