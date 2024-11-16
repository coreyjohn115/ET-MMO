using System;

namespace ET
{
    public class TimeInfo: Singleton<TimeInfo>, ISingletonAwake
    {
        private int timeZone;

        public int TimeZone
        {
            get => this.timeZone;
            set
            {
                this.timeZone = value;
                dt = dt1970.AddHours(this.timeZone);
            }
        }

        private DateTime dt1970;
        private DateTime dt;
        
        // ping消息会设置该值，原子操作
        public long ServerMinusClientTime { private get; set; }

        /// <summary>
        /// 当前时间戳(MS)
        /// </summary>
        public long Frame { get; private set; }

        /// <summary>
        /// 当前时间戳
        /// </summary>
        public long Second => Frame / 1000L;

        public void Awake()
        {
            this.dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(System.DateTime.UtcNow);
            TimeZone = (int)offset.TotalHours;
            this.Frame = this.Now();
        }

        public void Update()
        {
            // 赋值long型是原子操作，线程安全
            this.Frame = this.Now();
            Log.Console(this.Frame);
        }

        /// <summary> 
        /// 根据时间戳获取时间 
        /// </summary>  
        public DateTime DateTime(long? timeStamp = null)
        {
            timeStamp ??= Second;
            return dt.AddTicks(timeStamp.Value * 10000_000L);
        }

        public string DateString(long? timeStamp = null, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return this.DateTime(timeStamp).ToString(format);
        }

        // 线程安全
        public long Now()
        {
            return (System.DateTime.Now.Ticks - this.dt.Ticks) / 10000L;
        }
        
        /// <summary>
        /// 服务器当前时间
        /// </summary>
        /// <returns></returns>
        public long ServerNow()
        {
            return Now() + this.ServerMinusClientTime;
        }

        public long ServerFrame()
        {
            return this.Frame + this.ServerMinusClientTime;
        }

        public long Transition(DateTime d)
        {
            return (d.Ticks - dt.Ticks) / 10000L;
        }

        public long ToUnixTimeSeconds(DateTime dateTime)
        {
            return (dateTime.Ticks - this.dt.Ticks) / 10000_000L;
        }

        public int GetSecond()
        {
            return (int)Second;
        }

        public int GetMinute()
        {
            return (int)(this.Frame / 1000L / 60L);
        }

        public int GetHour()
        {
            return (int)(this.Frame / 1000L / 3600L);
        }

        /// <summary>
        /// 获取utc零点到今天的天数
        /// </summary>
        /// <returns></returns>
        public int GetDay()
        {
            return (int)(this.Frame / 1000L / 3600L / 24L);
        }

        /// <summary>
        /// 获取当天的秒数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetSecondSinceZero(long time)
        {
            var d = this.DateTime(time);
            return d.Hour * 3600 + d.Minute * 60 + d.Second;
        }

        /// <summary>
        /// 获取当天零点的时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetZeroSecond(long? time = null)
        {
            time ??= Second;
            return (int)(time - GetSecondSinceZero(time.Value));
        }
    }
}