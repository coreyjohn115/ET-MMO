namespace ET.Server
{
    // 离开视野
    [Event(SceneType.Map)]
    public class UnitLeaveSightRange_Update: AEvent<Scene, UnitLeaveSightRange>
    {
        protected override async ETTask Run(Scene scene, UnitLeaveSightRange args)
        {
            await ETTask.CompletedTask;
            AOIEntity a = args.A;
            AOIEntity b = args.B;
            if (a.Unit.Type() != UnitType.Player)
            {
                return;
            }

            MapHelper.NoticeUnitRemove(a.Unit, b.Unit);
        }
    }
}