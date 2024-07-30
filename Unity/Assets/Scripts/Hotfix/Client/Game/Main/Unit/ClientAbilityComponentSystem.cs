namespace ET.Client
{
    [FriendOf(typeof (ClientAbilityComponent))]
    [EntitySystemOf(typeof (ClientAbilityComponent))]
    public static partial class ClientAbilityComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientAbilityComponent self)
        {
            self.AddAbility(RoleAbility.Attack);
            self.AddAbility(RoleAbility.UseItem);
            self.AddAbility(RoleAbility.Skill);
            self.AddAbility(RoleAbility.Move);
        }

        public static void UpdateAbility(this ClientAbilityComponent self, int ability)
        {
            self.serverValue = ability;
        }

        public static void AddAbility(this ClientAbilityComponent self, RoleAbility ability)
        {
            self.AddAbility((int)ability);
        }

        public static void AddAbility(this ClientAbilityComponent self, int ability)
        {
            self.abilityList[ability] += 1;
            int value = 0;
            for (var i = 1; i < self.abilityList.Length; i++)
            {
                var v = self.abilityList[i];
                if (v != 0)
                {
                    value |= 1 << i;
                }
            }

            self.value = value;
        }

        public static void RemoveAbility(this ClientAbilityComponent self, RoleAbility ability)
        {
            self.RemoveAbility((int)ability);
        }

        public static void RemoveAbility(this ClientAbilityComponent self, int ability)
        {
            self.abilityList[ability] -= 1;
            int value = self.value;
            for (var i = 1; i < self.abilityList.Length; i++)
            {
                var v = self.abilityList[i];
                if (v == 0)
                {
                    value &= ~(1 << i);
                }
            }

            self.value = value;
        }

        public static bool HasAbility(this ClientAbilityComponent self, int ability)
        {
            return (self.value & (1 << ability)) != 0 && (self.serverValue & (1 << ability)) != 0;
        }

        public static bool HasAbility(this ClientAbilityComponent self, RoleAbility ability)
        {
            return self.HasAbility((int)ability);
        }

        /// <summary>
        /// 能否普攻
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool CanNormalAttack(this Unit self)
        {
            return self.GetComponent<ClientAbilityComponent>().HasAbility(RoleAbility.Attack);
        }

        /// <summary>
        /// 能否使用技能
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool CanSkill(this Unit self)
        {
            return self.GetComponent<ClientAbilityComponent>().HasAbility(RoleAbility.Skill);
        }

        /// <summary>
        /// 能否移动
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool CanMove(this Unit self)
        {
            return self.GetComponent<ClientAbilityComponent>().HasAbility(RoleAbility.Move);
        }
    }
}