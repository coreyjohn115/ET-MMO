namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterUnitCreate_AddComponent: AEvent<Scene, AfterUnitCreate>
    {
        protected override async ETTask Run(Scene scene, AfterUnitCreate e)
        {
            await ETTask.CompletedTask;
            UnitType t = (UnitType)e.Unit.Config().Type;
            switch (t)
            {
                case UnitType.Player:
                    if (e.IsMainPlayer)
                    {
                        e.Unit.AddComponent<UnitLucky>();
                    }

                    e.Unit.AddComponent<UnitBasic>();
                    e.Unit.AddComponent<FashionComponent>();
                    e.Unit.AddComponent<PathfindingComponent, string>(scene.Name);
                    e.Unit.AddComponent<ClientShieldComponent>();
                    e.Unit.AddComponent<ClientSkillComponent>();
                    e.Unit.AddComponent<ClientBuffComponent>();
                    e.Unit.AddComponent<ClientAbilityComponent>().UpdateAbility(e.UnitInfo.FightData.Ability);
                    break;
            }
        }
    }
}