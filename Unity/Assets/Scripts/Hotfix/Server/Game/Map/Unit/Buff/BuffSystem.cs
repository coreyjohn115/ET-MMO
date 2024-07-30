namespace ET.Server
{
    [EntitySystemOf(typeof (BuffUnit))]
    public static partial class BuffSystem
    {
        [EntitySystem]
        private static void Awake(this BuffUnit self, int id, long time, long addUnitId)
        {
            self.BuffId = id;
            self.AddTime = time;
            self.AddRoleId = addUnitId;
        }
    }
}