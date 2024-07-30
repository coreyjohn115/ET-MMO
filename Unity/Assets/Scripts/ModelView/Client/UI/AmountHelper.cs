using System;
using UnityEngine;

namespace ET.Client
{
    public enum AmountType
    {
        /// <summary>
        /// 100000 -> 10万  ， 100000000 -> 1亿）
        /// </summary>
        NormalUnit,

        /// <summary>
        /// 1000 -> 1k ,M ,B ,T等等
        /// </summary>
        BigUnit,

        /// <summary>
        /// 金融计数方式（ 1000000 ->   1,000,000
        /// </summary>
        FinanceUnit
    };

    // 数量显示视图
    public static class AmountHelper
    {
        private struct Number
        {
            public Number(string amount, int count)
            {
                this.Amount = amount;
                this.Count = count;
            }

            /// <summary>
            /// 结果
            /// </summary>
            public string Amount;

            /// <summary>
            /// 循环次数
            /// </summary>
            public int Count;
        }

        private const int numLangId = 106010;
        private const int wanLangId = 106011;
        private const int yiLangId = 106012;

        public static string GetAmountText(double amount, out Color color, AmountType t = AmountType.NormalUnit)
        {
            color = Color.black;
            switch (t)
            {
                case AmountType.NormalUnit:
                    if (amount > 1_0000_0000)
                    {
                        var langCfg = LanguageCategory.Instance.Get(yiLangId);
                        color = langCfg.ColorBytes.BytesColor();
                        return langCfg.Fmt(Math.Floor(amount / 1_000_0000) / 10.0);
                    }

                    if (amount > 10_0000)
                    {
                        var langCfg = LanguageCategory.Instance.Get(wanLangId);
                        color = langCfg.ColorBytes.BytesColor();
                        return langCfg.Fmt(Math.Floor(amount / 1000) / 10.0);
                    }
                    else
                    {
                        var langCfg = LanguageCategory.Instance.Get(numLangId);
                        color = langCfg.ColorBytes.BytesColor();
                        return langCfg.Fmt(amount);
                    }
                case AmountType.BigUnit:
                    return GetAmountTextUnit(amount);
                case AmountType.FinanceUnit:
                    return GetFinanceTextUnit(amount);
                default:
                    return amount.ToString();
            }
        }

        /// <summary>
        /// 将数值进行转换，格式a、b、c...
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static string GetAmountTextUnit(double amount)
        {
            Number number = DoNumber(Math.Abs(amount), 0);
            string re = number.Amount;
            switch (number.Count)
            {
                case 1:
                    re = $"{re}K";
                    break;
                case 2:
                    re = $"{re}M";
                    break;
                case 3:
                    re = $"{re}B";
                    break;
                case 4:
                    re = $"{re}T";
                    break;
                case 5:
                    re = $"{re}aa";
                    break;
                case 6:
                    re = $"{re}bb";
                    break;
                case 7:
                    re = $"{re}cc";
                    break;
                case 8:
                    re = $"{re}dd";
                    break;
                case 9:
                    re = $"{re}ee";
                    break;
                case 10:
                    re = $"{re}ff";
                    break;
            }

            if (amount < 0)
            {
                re = "-" + re;
            }

            return re;
        }

        private static string GetFinanceTextUnit(double amount)
        {
            string finance = $"{amount:N0}";
            return finance;
        }

        private static Number DoNumber(double amount, int count)
        {
            while (true)
            {
                if (amount < 1000 || count >= 10) //上限为10次 最大单位为ff,超过以后不转
                {
                    string re = amount.ToString();
                    int index = re.IndexOf(".", StringComparison.Ordinal); //截取小数点4位
                    switch (index)
                    {
                        case <= 0:
                            return new Number(re, count);
                        case >= 3 when count >= 10:
                            re = re.Substring(0, index); //超过10次的保留小数点前所有数据
                            break;
                        case >= 3:
                            re = re.Substring(0, 3);
                            break;
                        default:
                            re = re.Substring(0, 4 > re.Length? re.Length : 4);
                            break;
                    }

                    return new Number(re, count);
                }

                amount /= 1000;
                count++;
            }
        }
    }
}