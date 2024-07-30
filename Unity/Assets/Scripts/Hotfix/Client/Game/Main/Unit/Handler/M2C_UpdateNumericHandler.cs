namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateNumericHandler: MessageHandler<Scene, M2C_UpdateNumeric>
    {
        protected override async ETTask Run(Scene root, M2C_UpdateNumeric message)
        {
            var unit = UnitHelper.GetMyUnitFromClientScene(root);
            if (!unit)
            {
                return;
            }

            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            foreach (var kv in message.KV)
            {
                numericComponent.Set(kv.Key, kv.Value);
            }

            await ETTask.CompletedTask;
        }
    }
}