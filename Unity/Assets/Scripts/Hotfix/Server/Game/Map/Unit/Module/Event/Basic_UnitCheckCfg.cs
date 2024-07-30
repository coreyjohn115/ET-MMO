namespace ET.Server;

[Event(SceneType.Map)]
public class Basic_UnitCheckCfg: AEvent<Scene, UnitCheckCfg>
{
    protected override async ETTask Run(Scene scene, UnitCheckCfg a)
    {
        UnitBasic basic = a.Unit.GetComponent<UnitBasic>();
        var levelCfg = PlayerLevelConfigCategory.Instance.Get(basic.Level);
        a.Unit.AddAttrList(levelCfg.AttrList);

        await ETTask.CompletedTask;
    }
}