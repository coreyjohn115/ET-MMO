using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public struct MessageReturn
    {
        public int Errno;

        public List<string> Message;

        public static MessageReturn Create(int errco, List<string> message = default) => new() { Errno = errco, Message = message ?? new(), };

        public static MessageReturn Success() => new() { Errno = ErrorCode.ERR_Success, Message = new(), };
    }

    public interface IMessageSessionHandler
    {
        void Handle(Session session, object message);

        Type GetMessageType();

        Type GetResponseType();
    }
}