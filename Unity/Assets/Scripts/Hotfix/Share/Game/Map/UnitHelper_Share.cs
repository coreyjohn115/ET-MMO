using System.Collections.Generic;

namespace ET
{
    public static class UnitHelper_Share
    {
        public static bool IsAlive(this Unit self)
        {
            if (self == null || self.IsDisposed)
            {
                return false;
            }

            NumericComponent numeric = self.GetComponent<NumericComponent>();
            return numeric.GetAsLong(NumericType.Hp) > 0;
        }

        public static void AddAttrList(this Unit self, List<AttrArgs> list)
        {
            NumericComponent numeric = self.GetComponent<NumericComponent>();
            foreach (AttrArgs args in list)
            {
                numeric.SetNoEvent(args.AttrType, args.AttrValue);
            }
        }

        /// <summary>
        /// 获取当前角色的的属性值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="attrType">属性类型</param>
        /// <returns></returns>
        public static long GetAttrValue(this Unit self, int attrType)
        {
            NumericComponent numeric = self.GetComponent<NumericComponent>();
            return numeric.GetAsLong(attrType);
        }
    }
}