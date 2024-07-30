using System;

namespace ET.Server;

[MessageHandler(SceneType.All)]
public class W2Other_GCHandler: MessageHandler<Scene, W2Other_GCRequest, Other2W_GCResponse>
{
    protected override async ETTask Run(Scene scene, W2Other_GCRequest request, Other2W_GCResponse response)
    {
        Log.Console("开始GC。。。");
        GC.Collect();
        Log.Console("结束GC。。。");
        await ETTask.CompletedTask;
    }
}