namespace ET.Server
{
    [EntitySystemOf(typeof (UnitLucky))]
    public static partial class UnitLuckySystem
    {
        [EntitySystem]
        private static void Awake(this UnitLucky self)
        {
        }

        private static void DataChange(this UnitLucky self)
        {
            self.UpdateCache().NoContext();
        }
    }
}