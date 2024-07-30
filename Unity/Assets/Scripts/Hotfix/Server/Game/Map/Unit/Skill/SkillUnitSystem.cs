namespace ET.Server;

[EntitySystemOf(typeof (SkillUnit))]
[FriendOf(typeof (SkillUnit))]
public static partial class SkillUnitSystem
{
    [EntitySystem]
    private static void Awake(this SkillUnit self)
    {
        self.layer = self.MasterConfig.MaxLayer;
        for (int i = self.cdList.Count; i < self.layer; i++)
        {
            self.cdList.Add(TimeInfo.Instance.FrameTime);
        }
    }

    public static SkillProto ToSkillProto(this SkillUnit self)
    {
        var skill = SkillProto.Create();
        skill.Id = (int)self.Id;
        skill.CdList.AddRange(self.cdList);
        skill.Level = self.level;
        return skill;
    }

    /// <summary>
    /// 更新客户端单个技能
    /// </summary>
    /// <param name="self"></param>
    /// <param name="unit"></param>
    public static void UpdateSkill(this SkillUnit self, Unit unit)
    {
        M2C_UpdateSkill pkg = M2C_UpdateSkill.Create();
        pkg.Skill = self.ToSkillProto();
        unit.SendToClient(pkg).NoContext();
    }

    public static void LevelUp(this SkillUnit self, int level)
    {
        self.level = level;
    }
}