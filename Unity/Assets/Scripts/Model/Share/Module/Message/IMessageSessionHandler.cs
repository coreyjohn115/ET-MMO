using System;
using System.Collections.Generic;

namespace ET
{
    public struct MessageReturn
    {
        public int Errno;

        public List<string> Message;

        public static MessageReturn Create(int errno, List<string> message = default) => new() { Errno = errno, Message = message ?? new(), };

        public static MessageReturn Success() => new() { Errno = ErrorCode.ERR_Success, Message = new(), };
    }

    public interface IMessageSessionHandler
    {
        void Handle(Session session, object message);

        Type GetMessageType();

        Type GetResponseType();
    }
}