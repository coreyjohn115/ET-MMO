namespace ET.Client
{
    /// <summary>
    /// 飘受伤
    /// </summary>
    [Event(SceneType.Current)]
    public class PopHurtHud_Handler: AEvent<Scene, PopHurtHud>
    {
        protected override async ETTask Run(Scene scene, PopHurtHud a)
        {
            scene.GetComponent<BattleText>().PopHud(a.Id, a.Dest, a.Info).NoContext();

            await ETTask.CompletedTask;
        }
    }
}