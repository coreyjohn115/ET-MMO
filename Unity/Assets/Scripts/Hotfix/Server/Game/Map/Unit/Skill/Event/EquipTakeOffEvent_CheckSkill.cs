namespace ET.Server;

[Event(SceneType.Map)]
public class EquipTakeOffEvent_CheckSkill: AEvent<Scene, EquipTakeOffEvent>
{
    protected override async ETTask Run(Scene scene, EquipTakeOffEvent a)
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
            unit.GetComponent<SkillComponent>().RemoveSkill(config.Skill);
        }

        await ETTask.CompletedTask;
    }
}