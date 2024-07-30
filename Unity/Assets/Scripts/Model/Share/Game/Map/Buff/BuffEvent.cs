namespace ET
{
    public enum BuffEvent
    {
         /// <summary>
        /// 对象死亡
        /// </summary>
        ObjectDead = 5,
        
        /// <summary>
        /// 对象复活
        /// </summary>
        ObjectRelive = 7,
        
        /// <summary>
        /// 受到物理伤害
        /// </summary>
        HurtPhysics = 8,
        
        /// <summary>
        /// 受到魔法伤害
        /// </summary>
        HurtMagic = 9,
        
        /// <summary>
        /// 闪避时(受伤者闪避)
        /// </summary>
        BeMiss = 11,
        
        /// <summary>
        /// 闪避时(自己闪避)
        /// </summary>
        Miss = 12,
        
        /// <summary>
        /// 暴击时(受伤者闪避)
        /// </summary>
        BeHurtCirt = 13,
        
        /// <summary>
        /// 暴击时(自己暴击)
        /// </summary>
        HurtCirt = 14,
        
        /// <summary>
        /// 被控制
        /// </summary>
        BeCtrl = 15,
        
        /// <summary>
        /// 产生伤害
        /// </summary>
        Hurt = 16,
        
        /// <summary>
        /// 产生伤害前
        /// </summary>
        PerHurt = 17,
        
        /// <summary>
        /// 格挡(受伤者格挡)
        /// </summary>
        BeFender = 18,
        
        /// <summary>
        /// 格挡(自己格挡)
        /// </summary>
        Fender = 19,
        
        /// <summary>
        /// 反伤(受伤者)
        /// </summary>
        BackHurt = 20,
        
        /// <summary>
        /// 获得技能
        /// </summary>
        AddSkill = 21,
        
        /// <summary>
        /// 加血
        /// </summary>
        AddHp = 25,
        
        /// <summary>
        /// 使用普通技能
        /// </summary>
        UseNormal = 27,
        
        /// <summary>
        /// 使用技能
        /// </summary>
        UseSkill = 28,
        
        /// <summary>
        /// 进入地图
        /// </summary>
        EnterMap = 30,
        
        /// <summary>
        /// 进入地图
        /// </summary>
        LeaveMap = 31,
        
        /// <summary>
        /// 打断别人的技能
        /// </summary>
        BreakSkill = 32,

        /// <summary>
        /// 对护盾造成伤害
        /// </summary>
        HurtShield = 33,

        /// <summary>
        /// 打破护盾
        /// </summary>
        BreakShield = 34,

        /// <summary>
        /// 受到移动速度限制
        /// </summary>
        BeSpeed = 35,

        /// <summary>
        /// 受到伤害
        /// </summary>
        BeHurt = 36,
    }
}