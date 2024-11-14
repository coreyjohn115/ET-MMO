using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Unity.Mathematics;

namespace ET.Server
{
    public enum OriginMode
    {
        Actor,
        Parent,
    }

    public enum SharpType
    {
        /// <summary>
        /// 直线(矩形)
        /// </summary>
        Line = 1,

        /// <summary>
        /// 圆形
        /// </summary>
        Round = 2,

        /// <summary>
        /// 扇形
        /// </summary>
        Fan = 3,
    }

    /// <summary>
    /// Unit 选取器
    /// </summary>
    public static class UnitFilter
    {
        private static bool CalcPointLine(double tan, double x0, double y0)
        {
            return tan * x0 - y0 <= 0;
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Floor(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        /// <summary>
        /// 点是否在矩形范围内
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxZ"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static bool InRect(double minX, double minY, double maxX, double maxZ, float3 point)
        {
            return point.x >= minX && point.x <= maxX && point.z >= minY && point.z <= maxZ;
        }

        public static float Angle(float3 from, float3 to)
        {
            math.normalize(from);
            math.normalize(to);
            float result = math.dot(from, to);
            return math.acos(math.clamp(result, -1f, 1f)) * 57.29578f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitList"></param>
        /// <param name="srcX"></param>
        /// <param name="srcZ"></param>
        /// <param name="sharp"></param>
        /// <param name="forward"></param>
        /// <param name="repairPos"></param>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        /// <param name="dstSet"></param>
        /// <returns></returns>
        private static HashSet<Unit> GetSelector(List<Unit> unitList, float srcX, float srcZ, SharpType sharp, float3 forward, float repairPos,
        float args1, float args2, HashSet<long> dstSet = default)
        {
            while (true)
            {
                HashSet<Unit> list = [];
                var repair = UnitHelper.GetRepairPos(srcX, srcZ, forward, repairPos);
                srcX = repair.Key;
                srcZ = repair.Value;
                switch (sharp)
                {
                    case SharpType.Line:
                        double cosX = Math.Cos(Math.Atan2(forward.z, forward.x));
                        double sinX = Math.Sin(Math.Atan2(forward.z, forward.x));
                        foreach (var unit in unitList)
                        {
                            var modelR = unit.Config().ModelR / 100;
                            if (dstSet != null && dstSet.Contains(unit.Id))
                            {
                                modelR += 5;
                            }

                            float dstX0 = unit.Position.x - srcX;
                            float dstZ0 = unit.Position.z - srcZ;
                            double dstX = dstX0 * cosX - dstZ0 * sinX;
                            double dstZ = dstX0 * sinX + dstZ0 * cosX;
                            if (dstX <= args1 + modelR && dstX >= -modelR && dstZ <= (args2 + modelR) / 2 && dstZ >= -(args2 + modelR) / 2)
                            {
                                list.Add(unit);
                            }
                        }

                        break;
                    case SharpType.Round:
                        foreach (Unit unit in unitList)
                        {
                            int modelR = unit.Config().ModelR / 100;
                            if (dstSet != null && dstSet.Contains(unit.Id))
                            {
                                modelR += 5;
                            }

                            if (GetDistance(srcX, srcZ, unit.Position.x, unit.Position.z) <= Math.Pow(args1 + modelR, 2))
                            {
                                list.Add(unit);
                            }
                        }

                        break;
                    case SharpType.Fan:
                        args2 %= 360;
                        if (args2 == 0)
                        {
                            sharp = SharpType.Round;
                            continue;
                        }

                        float3 curPos = new(srcX, 0, srcZ);
                        forward = math.normalize(forward);
                        foreach (Unit unit in unitList)
                        {
                            curPos.y = unit.Position.y;
                            float3 targetDirection = math.normalize(unit.Position - curPos);
                            float angle = Angle(forward, targetDirection);
                            int modelR = unit.Config().ModelR / 100;
                            if (math.distancesq(curPos, unit.Position) <= Math.Pow(args1 + modelR, 2) && angle <= args2 / 2)
                            {
                                list.Add(unit);
                            }
                        }

                        break;
                }

                return list;
            }
        }

        /// <summary>
        /// 获取攻击列表
        /// </summary>
        /// <param name="self">攻击者</param>
        /// <param name="fT">攻击目标类型</param>
        /// <param name="rT">攻击范围类型</param>
        /// <param name="forward">攻击者朝向</param>
        /// <param name="dstList">选中目标列表</param>
        /// <param name="dstPosList">选中位置列表</param>
        /// <param name="repairPos">修改自身位置</param>
        /// <param name="args1">攻击范围是扇形时表示半径, 直线时表示长度</param>
        /// <param name="args2">攻击范围是扇形时表示角度, 直线时表示宽度</param>
        /// <param name="args3"></param>
        /// <param name="args4"></param>
        /// <param name="maxDistance">最大距离</param>
        /// <returns>符合条件的对象列表</returns>
        public static HashSet<Unit> GetAttackList(this Unit self,
        FocusType fT,
        RangeType rT,
        float3 forward,
        List<Unit> dstList,
        List<float3> dstPosList,
        float repairPos,
        float args1, int args2, float args3, float args4,
        float maxDistance)
        {
            HashSet<long> dstSet = [];
            if (dstList != null && self.Type() == UnitType.Player)
            {
                foreach (Unit unit in dstList)
                {
                    dstSet.Add(unit.Id);
                }
            }

            HashSet<Unit> set = [];
            int maxCount = 99999;
            HashSet<Unit> set2 = [];
            switch (rT)
            {
                case RangeType.Self:
                {
                    set2.Add(self);
                    break;
                }
                case RangeType.Single:
                {
                    maxCount = 1;
                    set2 = GetSelector(dstList, self.Position.x, self.Position.z, SharpType.Round, forward, repairPos, maxDistance * 1.2f, 0,
                        dstSet);
                    break;
                }
                case RangeType.SelfFan:
                {
                    var ll = self.GetFocusPlayers(fT);
                    set2 = GetSelector(ll, self.Position.x, self.Position.z, SharpType.Fan, forward, repairPos, args1, args2);
                    break;
                }
                case RangeType.SelfLine:
                {
                    var ll = self.GetFocusPlayers(fT);
                    set2 = GetSelector(ll, self.Position.x, self.Position.z, SharpType.Line, forward, repairPos, args1, args2);
                    break;
                }
                case RangeType.DstLine:
                {
                    var ll = self.GetFocusPlayers(fT);
                    foreach (var pos in dstPosList)
                    {
                        var rList = GetSelector(ll, pos.x, pos.z, SharpType.Line, forward, repairPos, args1, args2);
                        set2.AddRange(rList);
                    }

                    break;
                }
                case RangeType.DstFan:
                {
                    var ll = self.GetFocusPlayers(fT);
                    foreach (var pos in dstPosList)
                    {
                        var rList = GetSelector(ll, pos.x, pos.z, SharpType.Fan, forward, repairPos, args1, args2);
                        set2.AddRange(rList);
                    }

                    break;
                }
                case RangeType.DstFanLine:
                {
                    var ll = self.GetFocusPlayers(fT);
                    var ll1 = GetSelector(ll, self.Position.x, self.Position.z, SharpType.Fan, forward, repairPos, args1, args2);
                    var ll2 = GetSelector(ll, self.Position.x, self.Position.z, SharpType.Line, forward, repairPos, args3, args4);
                    set2.AddRange(ll1);
                    set2.AddRange(ll2);

                    break;
                }
                case RangeType.UseLast:
                    break;
            }

            int count = 0;
            HashSet<long> unitMap = [];
            foreach (var unit in set2)
            {
                if (!unitMap.Contains(unit.Id))
                {
                    set.Add(unit);
                    unitMap.Add(unit.Id);
                    count += 1;
                    if (count >= maxCount)
                    {
                        break;
                    }
                }
            }

            return set;
        }
    }
}