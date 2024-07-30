namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateItemHandler: MessageHandler<Scene, M2C_UpdateItem>
    {
        protected override async ETTask Run(Scene entity, M2C_UpdateItem message)
        {
            if (message.IsDelete)
            {
                entity.GetComponent<ClientItemComponent>().RemoveItem(message.Id);
            }
            else
            {
                ItemData d = MongoHelper.Deserialize<ItemData>(message.Item);
                entity.GetComponent<ClientItemComponent>().AddUpdateItem(message.Id, d);
            }

            await ETTask.CompletedTask;
        }
    }
}