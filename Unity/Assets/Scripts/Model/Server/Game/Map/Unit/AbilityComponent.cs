namespace ET.Server
{
    [UnitCom]
    [ComponentOf(typeof (Unit))]
    public class AbilityComponent: Entity, IAwake, ITransfer
    {
        public int Ability => this.value;

        /// <summary>
        /// 能力值
        /// </summary>
        public int value;

        /// <summary>
        /// 能力列表
        /// </summary>
        public int[] abilityList = new int[(int)RoleAbility.End];
    }
}