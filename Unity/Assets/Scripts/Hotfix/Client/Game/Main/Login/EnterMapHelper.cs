using System;

namespace ET.Client
{
    public static partial class EnterMapHelper
    {
        public static async ETTask<int> EnterMapAsync(Scene root)
        {
            try
            {
                G2C_EnterMap g2CEnterMap = await root.GetComponent<ClientSenderComponent>().Call(C2G_EnterMap.Create()) as G2C_EnterMap;

                if (g2CEnterMap.Error != ErrorCode.ERR_Success)
                {
                    return g2CEnterMap.Error;
                }

                // 等待场景切换完成
                await root.GetComponent<ObjectWait>().Wait<Wait_SceneChangeFinish>();

                await EventSystem.Instance.PublishAsync(root, new EnterMapFinish());
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return ErrorCode.ERR_Success;
        }
    }
}