namespace ET.Client
{
    [ChildOf(typeof (ActionComponent))]
    public class ActionUnit: Entity, IAwake<string, float>, IDestroy
    {
        public string ActionName { get; set; }

        public ETTask tcs;
    }
}