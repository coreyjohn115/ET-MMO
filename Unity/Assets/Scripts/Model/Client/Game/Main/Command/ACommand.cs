namespace ET.Client
{
    public abstract class ACommand: Object
    {
        public bool Exited { get; protected set; }

        public abstract ETTask Run(CommandUnit self, CommandData data);

        public virtual void Exit(CommandUnit self)
        {
            this.Exited = true;
        }
    }
}