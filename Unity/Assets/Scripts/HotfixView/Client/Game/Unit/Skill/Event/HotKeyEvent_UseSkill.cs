using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class HotKeyEvent_UseSkill: AEvent<Scene, HotKeyEvent>
    {
        protected override async ETTask Run(Scene scene, HotKeyEvent e)
        {
            switch (e.KeyCode)
            {
                case KeyCodeConst.NormalAttack:
                    await UnitHelper.GetMyUnitFromCurrentScene(scene).GetComponent<ClientSkillComponent>().NormalAttack();
                    break;
                case KeyCodeConst.Skill1:
                case KeyCodeConst.Skill2:
                case KeyCodeConst.Skill3:
                case KeyCodeConst.Skill4:
                case KeyCodeConst.Skill5:
                case KeyCodeConst.Skill6:
                    await UnitHelper.GetMyUnitFromCurrentScene(scene).GetComponent<ClientSkillComponent>().UseSkill(0);
                    break;
            }
        }
    }
}