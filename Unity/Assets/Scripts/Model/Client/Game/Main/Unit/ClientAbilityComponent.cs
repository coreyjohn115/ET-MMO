namespace ET.Client
{
    public struct FightStateChange
    {
        public bool IsBattle;
        public Unit Unit;
    }

    [ComponentOf(typeof (Unit))]
    public class ClientAbilityComponent: Entity, IAwake
    {
        /// <summary>
        /// 是否在战斗状态
        /// </summary>
        public bool IsBattle
        {
            get
            {
                return this.isBattle;
            }
            set
            {
                EventSystem.Instance.Publish(this.Scene(),
                    new FightStateChange() { Unit = this.GetParent<Unit>(), IsBattle = value });
                this.isBattle = value;
            }
        }

        private bool isBattle;

        /// <summary>
        /// 本地能力值
        /// </summary>
        public int value;

        /// <summary>
        /// 本地能力列表
        /// </summary>
        public int[] abilityList = new int[(int)RoleAbility.End];

        /// <summary>
        /// 服务器能力值
        /// </summary>
        public int serverValue;
    }
}