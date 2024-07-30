namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateEquipHandler: MessageHandler<Scene, M2C_UpdateEquip>
    {
        protected override async ETTask Run(Scene scene, M2C_UpdateEquip message)
        {
            scene.GetComponent<ClientEquipComponent>().UpdateEquip(message.EquipDict);
            await ETTask.CompletedTask;
        }
    } 
}