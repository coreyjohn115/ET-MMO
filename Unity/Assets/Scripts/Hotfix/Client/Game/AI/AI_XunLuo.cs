using Unity.Mathematics;

namespace ET.Client
{
    public class AI_XunLuo: AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            long sec = TimeInfo.Instance.ServerFrameTime() / 1000L % 15L;
            if (sec < 10L)
            {
                return 0;
            }

            return 1;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig)
        {
            Scene root = aiComponent.Root();

            Unit myUnit = UnitHelper.GetMyUnitFromClientScene(root);
            if (myUnit == null)
            {
                return;
            }

            Log.Debug("开始巡逻");
            while (true)
            {
                XunLuoPathComponent xunLuoPathComponent = myUnit.GetComponent<XunLuoPathComponent>();
                float3 nextTarget = xunLuoPathComponent.GetCurrent();
                int error = await myUnit.MoveToAsync(nextTarget);
                var cancellationToken = await ETTaskHelper.GetContextAsync<ETCancellationToken>();
                if (error == WaitTypeError.Cancel || cancellationToken.IsCancel())
                {
                    return;
                }

                xunLuoPathComponent.MoveNext();
            }
        }
    }
}