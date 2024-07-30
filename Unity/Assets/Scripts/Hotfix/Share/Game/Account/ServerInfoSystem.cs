namespace ET
{
    public static class ServerInfoSystem
    {
        public static void FromMessage(this ServerInfo self, ServerInfoProto serverInfoProto)
        {
            self.Status = (ServerStatus)serverInfoProto.Status;
            self.ServerName = serverInfoProto.ServerName;
        }

        public static ServerInfoProto ToMessage(this ServerInfo self)
        {
            ServerInfoProto proto = ServerInfoProto.Create();
            proto.Id = self.Id;
            proto.ServerName = self.ServerName;
            proto.Status = (int)self.Status;
            return proto;
        }
    }
}