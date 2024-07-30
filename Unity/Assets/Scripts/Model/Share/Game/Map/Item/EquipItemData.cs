namespace ET
{
    public enum EquipWeaponType
    {
        /// <summary>
        /// 剑盾
        /// </summary>
        Shield = 1,

        /// <summary>
        /// 长剑
        /// </summary>
        Sword = 2,

        /// <summary>
        /// 枪
        /// </summary>
        Gun = 3,

        /// <summary>
        /// 法杖
        /// </summary>
        Wand = 4,
    }

    public enum EquipPosType
    {
        /// <summary>
        /// 武器
        /// </summary>
        Weapon = 1,

        /// <summary>
        /// 头盔
        /// </summary>
        Helm = 2,

        /// <summary>
        /// 衣服
        /// </summary>
        Clothes = 3,

        /// <summary>
        /// 项链
        /// </summary>
        Neck = 4,

        /// <summary>
        /// 裤子
        /// </summary>
        Trousers = 5,

        /// <summary>
        /// 鞋子
        /// </summary>
        Shoe = 6,

        /// <summary>
        /// 占位
        /// </summary>
        XX1 = 7,
        XX2 = 8,
        XX3 = 9,
        XX4 = 10,
        XX5 = 11,
        XX6 = 12,
    }

    [ComponentOf(typeof (ItemData))]
    public class EquipItemData: Entity, IAwake, ISerializeToEntity
    {
    }
}