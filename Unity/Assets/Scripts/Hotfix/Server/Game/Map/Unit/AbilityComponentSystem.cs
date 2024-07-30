namespace ET.Server
{
    [EntitySystemOf(typeof (AbilityComponent))]
    [FriendOf(typeof (AbilityComponent))]
    public static partial class AbilityComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AbilityComponent self)
        {
            UnitType t = (UnitType)self.GetParent<Unit>().Config().Type;
            switch (t)
            {
                case UnitType.Player:
                    self.AddAbility(RoleAbility.Attack, false);
                    self.AddAbility(RoleAbility.Skill, false);
                    self.AddAbility(RoleAbility.UseItem, false);
                    self.AddAbility(RoleAbility.Move, false);
                    break;
            }
        }

        private static void UpdateClient(this AbilityComponent self)
        {
            M2C_UpdateFightDataInfo info = M2C_UpdateFightDataInfo.Create();
            info.RoleId = self.GetParent<Unit>().Id;
            info.FightData = FightDataInfo.Create();
            info.FightData.Ability = self.value;
        }

        public static void AddAbility(this AbilityComponent self, RoleAbility ability, bool update = true)
        {
            self.AddAbility((int)ability, update);
        }

        public static void AddAbility(this AbilityComponent self, int ability, bool update = true)
        {
            self.abilityList[ability] += 1;
            int value = 0;
            for (var i = 0; i < self.abilityList.Length; i++)
            {
                var v = self.abilityList[i];
                if (v != 0)
                {
                    value |= 1 << i;
                }
            }

            self.value = value;
            self.UpdateClient();
        }

        public static void RemoveAbility(this AbilityComponent self, RoleAbility ability, bool update = true)
        {
            self.RemoveAbility((int)ability, update);
        }

        public static void RemoveAbility(this AbilityComponent self, int ability, bool update = true)
        {
            self.abilityList[ability] -= 1;
            int value = self.value;
            for (var i = 0; i < self.abilityList.Length; i++)
            {
                var v = self.abilityList[i];
                if (v == 0)
                {
                    value &= ~(1 << i);
                }
            }

            self.value = value;
            self.UpdateClient();
        }

        public static bool HasAbility(this AbilityComponent self, int ability)
        {
            return (self.value & ability) == 0;
        }

        public static bool HasAbility(this AbilityComponent self, RoleAbility ability)
        {
            return (self.value & (int)ability) == 0;
        }

        /// <summary>
        /// 能否普攻
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool CanNormalAttack(this Unit self)
        {
            return self.GetComponent<AbilityComponent>().HasAbility(RoleAbility.Attack);
        }

        /// <summary>
        /// 是否无敌
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsInvincible(this Unit self)
        {
            return self.GetComponent<AbilityComponent>().HasAbility(RoleAbility.Invincible);
        }

        /// <summary>
        /// 能否使用技能
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool CanSkill(this Unit self)
        {
            return self.GetComponent<AbilityComponent>().HasAbility(RoleAbility.Skill);
        }

        /// <summary>
        /// 能否移动
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool CanMove(this Unit self)
        {
            return self.GetComponent<AbilityComponent>().HasAbility(RoleAbility.Move);
        }
    }
}