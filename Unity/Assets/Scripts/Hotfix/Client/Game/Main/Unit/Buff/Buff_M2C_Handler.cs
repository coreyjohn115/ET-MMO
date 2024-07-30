namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateBuffHandler: MessageHandler<Scene, M2C_UpdateBuff>
    {
        protected override async ETTask Run(Scene root, M2C_UpdateBuff message)
        {
            Unit unit = UnitHelper.GetUnitFromClientScene(root, message.RoleId);
            if (!unit)
            {
                return;
            }

            unit.GetComponent<ClientBuffComponent>().UpdateBuff(message);
            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(SceneType.Client)]
    public class M2C_DelBuffHandler: MessageHandler<Scene, M2C_DelBuff>
    {
        protected override async ETTask Run(Scene root, M2C_DelBuff message)
        {
            Unit unit = UnitHelper.GetUnitFromClientScene(root, message.RoleId);
            if (!unit)
            {
                return;
            }

            unit.GetComponent<ClientBuffComponent>().DelBuff(message.Id);
            await ETTask.CompletedTask;
        }
    }
}