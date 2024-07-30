namespace ET
{
    public static partial class ConstValue
    {
        public const int RouterHttpPort = 30300;
        public const int AccoutHttpPort = 18010;
        public const int SessionTimeoutTime = 30 * 1000;

        public const int RankPage = 30;
        
        /// <summary>
        /// 一天所表示的秒数
        /// </summary>
        public const int DaySec = 24 * 60 * 60;
        
        /// <summary>
        /// 一周所表示的秒数
        /// </summary>
        public const int WeekSec = 7 * DaySec;
        
        /// <summary>
        /// 一年所表示的秒数
        /// </summary>
        public const int YearSec = 365 * DaySec;
    }
}