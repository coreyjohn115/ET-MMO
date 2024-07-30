namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_UseSkillHandler: MessageHandler<Scene, M2C_UseSkill>
    {
        protected override async ETTask Run(Scene root, M2C_UseSkill message)
        {
            if (root.GetComponent<ClientPlayerComponent>().MyId == message.RoleId)
            {
                return;
            }

            Unit unit = UnitHelper.GetUnitFromClientScene(root, message.RoleId);
            if (unit)
            {
                unit.GetComponent<ClientSkillComponent>().SyncUseSkill(message);
            }

            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateSkillHandler: MessageHandler<Scene, M2C_UpdateSkill>
    {
        protected override async ETTask Run(Scene root, M2C_UpdateSkill message)
        {
            Unit unit = UnitHelper.GetMyUnitFromClientScene(root);
            unit.GetComponent<ClientSkillComponent>().UpdateSkill(message.Skill);
            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(SceneType.Client)]
    public class M2C_UpdateSkillListHandler: MessageHandler<Scene, M2C_UpdateSkillList>
    {
        protected override async ETTask Run(Scene root, M2C_UpdateSkillList message)
        {
            Unit unit = UnitHelper.GetMyUnitFromClientScene(root);
            unit.GetComponent<ClientSkillComponent>().UpdateSkillList(message.List);
            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(SceneType.Client)]
    public class M2C_DelSkillListHandler: MessageHandler<Scene, M2C_DelSkillList>
    {
        protected override async ETTask Run(Scene root, M2C_DelSkillList message)
        {
            Unit unit = UnitHelper.GetMyUnitFromClientScene(root);
            unit.GetComponent<ClientSkillComponent>().DelSkill(message.List);
            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(SceneType.Client)]
    public class M2C_BreakSkillHandler: MessageHandler<Scene, M2C_BreakSkill>
    {
        protected override async ETTask Run(Scene root, M2C_BreakSkill message)
        {
            Unit unit = UnitHelper.GetUnitFromClientScene(root, message.RoleId);
            if (unit)
            {
                unit.GetComponent<ClientSkillComponent>().BreakSkill(message.Id);
            }

            await ETTask.CompletedTask;
        }
    }
}