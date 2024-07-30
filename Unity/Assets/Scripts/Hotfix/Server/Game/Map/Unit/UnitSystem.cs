namespace ET.Server
{
    public static partial class UnitSystem
    {
        public static void BroadCastHurt(this Unit self, int id, HurtPkg pkg)
        {
            if (pkg.HurtInfos.Count == 0)
            {
                return;
            }

            M2C_HurtList hurtList = M2C_HurtList.Create();
            hurtList.Id = id;
            hurtList.RoleId = self.Id;
            hurtList.HurtList.AddRange(pkg.HurtInfos);
            hurtList.ViewCmd = pkg.ViewCmd;
            MapMessageHelper.Broadcast(self, hurtList);
        }

        public static void SyncBasicInfo(this Unit self)
        {
            var proto = self.GetComponent<UnitBasic>().GetBasicProto();
            M2C_UpdateBasicInfo pkg = M2C_UpdateBasicInfo.Create();
            pkg.UnitBasic = proto;
            self.SendToClient(pkg).NoContext();
        }
    }
}