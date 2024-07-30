namespace ET.Server
{
    [EntitySystemOf(typeof (LeagueComponent))]
    public static partial class LeagueComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LeagueComponent self)
        {
        }
    }
}