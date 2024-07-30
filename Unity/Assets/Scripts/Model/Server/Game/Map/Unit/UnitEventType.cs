using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 玩家进入游戏(可以推送消息)
    /// </summary>
    public struct UnitEnterGame
    {
        public Unit Unit;
    }
    
    /// <summary>
    /// 玩家离开游戏
    /// </summary>
    public struct UnitLeaveGame
    {
        public Unit Unit;
    }

    public struct UnitCheckCfg
    {
        public Unit Unit;
    }

    /// <summary>
    /// 重新计算玩家属性(上线后调用)
    /// </summary>
    public struct UnitReEffect
    {
        public Unit Unit;
    }

    /// <summary>
    /// 玩家进入游戏完成(客户端主动发送)
    /// </summary>
    public struct UnitEnterGameOk
    {
        public Unit Unit;
    }

    public struct UnitPerHurt
    {
        public Unit Attacker;
        public Unit Unit;
        public long Hurt;
        public long ShieldHurt;
        public int Id;
    }

    public struct UnitDead
    {
        public Unit Unit;
        public long Killer;
        public int Id;
    }

    /// <summary>
    /// 攻击事件
    /// </summary>
    public struct UnitDoAttack
    {
        public Unit Unit;
        public List<HurtInfo> HurtList;
        public bool IsPhysics;
        public int Element;
    }

    /// <summary>
    /// 对象血量改变
    /// </summary>
    public struct UnitHpChange
    {
        public Unit Unit;
    }

    /// <summary>
    /// 对象受伤
    /// </summary>
    public struct UnitBeHurt
    {
        public Unit Unit;
        public Unit Attacker;
        public long Hurt;
        public int Id;
    }

    /// <summary>
    /// 对象增加血量
    /// </summary>
    public struct UnitAddHp
    {
        public Unit Unit;
        public Unit Attacker;
        public long Hp;
        public long RealHp;
    }
}