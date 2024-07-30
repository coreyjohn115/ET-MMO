namespace ET
{
    public interface IItemConfig
    {
        int Id { get; set; }

        long Stack { get; }

        int Type { get; set; }

        int Quality { get; set; }

        int Name { get; set; }
        
        int Desc { get; set; }

        string Icon { get; set; }
    }
}