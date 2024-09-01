namespace ET.Server;

public static class ServerMessageHelper
{
    public static void SetValue(this IResponse response, MessageReturn r)
    {
        response.Error = r.Errno;
        response.Message.AddRange(r.Message);
    }
}