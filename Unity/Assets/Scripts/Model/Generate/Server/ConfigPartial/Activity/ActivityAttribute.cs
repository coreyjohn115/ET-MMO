namespace ET;

public class ActivityAttribute: BaseAttribute
{
    public ActivityType Activity { get; }

    public ActivityAttribute(ActivityType activity)
    {
        this.Activity = activity;
    }
}

public abstract class AActivityArgs
{
    public virtual void Misc(ActivityConfig self)
    {
    }

    public virtual ActivityDataListItem Data(ActivityConfig self, string sourceData, int k)
    {
        return default;
    }

    public virtual int Id(ActivityConfig self)
    {
        return 0;
    }

    public virtual void Finish(ActivityConfig self)
    {
    }
}