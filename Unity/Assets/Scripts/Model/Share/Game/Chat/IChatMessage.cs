namespace ET
{
    public interface IChatMessage: IMessage
    {
    }

    public interface IChatRequest: IChatMessage, IRequest
    {
    }

    public interface IChatResponse: IChatMessage, IResponse
    {
    }
}