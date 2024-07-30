namespace ET
{
    public class BuffAttribute : BaseAttribute
    {
        public string Cmd { get; }

        public BuffAttribute(string cmd)
        {
            Cmd = cmd;
        }
    }
}