namespace ET.Client
{
    [Action(ActionType.Animator)]
    public class AnimatorAction: AAction
    {
        public override async ETTask OnExecute(Unit unit, ActionUnit actionUnit)
        {
            var animation = unit.GetComponent<UnitGoComponent>().GetAnimator();
            if (!animation)
            {
                return;
            }

            bool lockMove = actionUnit.Config.Args[0].ToBool();
            if (lockMove)
            {
                unit.GetComponent<ClientAbilityComponent>().RemoveAbility(RoleAbility.Move);
                unit.GetComponent<MoveComponent>().Stop(false);
            }

            for (int i = 1; i < actionUnit.Config.Args.Length; i++)
            {
                string view = actionUnit.Config.Args[i];
                unit.GetComponent<ActionComponent>().PlayAction(view).NoContext();
            }

            unit.GetComponent<AnimatorComponent>().SetTrigger(actionUnit.ActionName);
            await ETTask.CompletedTask;
        }

        public override void OnUnExecute(Unit unit, ActionUnit actionUnit)
        {
            bool lockMove = actionUnit.Config.Args[0].ToBool();
            if (lockMove)
            {
                unit.GetComponent<ClientAbilityComponent>().AddAbility(RoleAbility.Move);
            }
        }
    }
}