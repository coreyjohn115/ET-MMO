namespace ET.Server;

[Event(SceneType.Map)]
public class AddItemEvent_AutoDress: AEvent<Scene, AddItemEvent>
{
    protected override async ETTask Run(Scene scene, AddItemEvent a)
    {
        Unit unit = a.Unit;
        if (!unit)
        {
            return;
        }

        ItemData item = a.Item;
        if (item.ItemType != ItemType.Equip)
        {
            return;
        }

        EquipConfig config = EquipConfigCategory.Instance.Get(item.Config.Id);
        if (unit.GetComponent<EquipComponent>().IsDressEquip(config.EquipPos))
        {
            return;
        }

        unit.GetComponent<EquipComponent>().PutOn(item.Id);
        await ETTask.CompletedTask;
    }
}