using System;
using System.Diagnostics;
using System.Net;

namespace ET.Server;

[HttpHandler(SceneType.Account, "/m")]
[FriendOf(typeof (Account))]
public class HttpMemoryHandler: IHttpHandler
{
    public async ETTask Handle(Scene scene, HttpListenerContext context)
    {
        Process currentProcess = Process.GetCurrentProcess();
        long memoryUsed = currentProcess.WorkingSet64 / 1024L; // 获取当前工作集内存
        long totalMemory = GC.GetTotalMemory(false) / 1024L;

        HttpHelper.Response(context, ($"Memory Used: {memoryUsed / 1024f:.000}MB", $"Total Memory Allocated: {totalMemory / 1024f:.000}MB"));
        await ETTask.CompletedTask;
    }
}