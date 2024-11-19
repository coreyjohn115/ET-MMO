namespace ET.Client
{
    [Action(ActionType.Animator)]
    public class AnimatorAction: AAction
    {
        public override async ETTask OnExecute(Unit unit, ActionSubUnit actionUnit)
        {
            bool lockMove = actionUnit.Config.Args[0].ToBool();
            if (lockMove)
            {
                unit.GetComponent<ClientAbilityComponent>().RemoveAbility(RoleAbility.Move);
                unit.GetComponent<MoveComponent>().Stop(false);
            }

            string name = actionUnit.Config.Args[1];
            unit.GetComponent<AnimationComponent>().PlayAnimReturnIdle(name);
            for (int i = 2; i < actionUnit.Config.Args.Length; i++)
            {
                string view = actionUnit.Config.Args[i];
                unit.GetComponent<ActionComponent>().PlayAction(view).NoContext();
            }

            await ETTask.CompletedTask;
        }

        public override void OnUnExecute(Unit unit, ActionSubUnit actionUnit)
        {
            bool lockMove = actionUnit.Config.Args[0].ToBool();
            if (lockMove)
            {
                unit.GetComponent<ClientAbilityComponent>().AddAbility(RoleAbility.Move);
            }
        }
    }
}