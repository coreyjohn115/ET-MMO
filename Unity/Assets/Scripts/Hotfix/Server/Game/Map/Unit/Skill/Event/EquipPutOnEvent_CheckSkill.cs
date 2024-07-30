namespace ET.Server;

[Event(SceneType.Map)]
public class EquipPutOnEvent_CheckSkill: AEvent<Scene, EquipPutOnEvent>
{
    protected override async ETTask Run(Scene scene, EquipPutOnEvent a)
    {
        Unit unit = a.Unit;
        if (!unit)
        {
            return;
        }

        ItemData item = a.Item;
        EquipConfig config = EquipConfigCategory.Instance.Get(item.Config.Id);
        if (config.Skill > 0)
        {
            unit.GetComponent<SkillComponent>().AddSkill(config.Skill);
        }

        await ETTask.CompletedTask;
    }
}