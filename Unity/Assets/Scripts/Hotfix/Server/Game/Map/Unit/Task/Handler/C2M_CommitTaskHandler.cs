namespace ET.Server;

[MessageLocationHandler(SceneType.Map)]
public class C2M_CommitTaskHandler: MessageLocationHandler<Unit, C2M_CommitTask, M2C_CommitTask>
{
    protected override async ETTask Run(Unit unit, C2M_CommitTask request, M2C_CommitTask response)
    {
        var result = unit.GetComponent<TaskComponent>().CommitTask(request.Id, LogDef.Client);
        response.Error = result.Errno;
        response.Message = result.Message;
        await ETTask.CompletedTask;
    }
}