using System;

namespace ET
{
    public class TimeInfo: Singleton<TimeInfo>, ISingletonAwake
    {
        private int timeZone;
        
        public int TimeZone
        {
            get
            {
                return this.timeZone;
            }
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
        ///  当前时间戳(MS级)
        /// </summary>
        public long FrameTime { get; private set; }
        
        public void Awake()
        {
            this.dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.FrameTime = this.ClientNow();
        }

        public void Update()
        {
            // 赋值long型是原子操作，线程安全
            this.FrameTime = this.ClientNow();
        }
        
        /// <summary> 
        /// 根据时间戳获取时间 
        /// </summary>  
        public DateTime Time(long? timeStamp = null)
        {
            timeStamp ??= FrameTime;
            return dt.AddTicks(timeStamp.Value * 10000L);
        }
        
        // 线程安全
        public long ClientNow()
        {
            return (DateTime.UtcNow.Ticks - this.dt1970.Ticks) / 10000L;
        }
        
        public long ClientFrameTime()
        {
            return this.FrameTime;
        }
        
        /// <summary>
        /// 服务器当前时间
        /// </summary>
        /// <returns></returns>
        public long ServerNow()
        {
            return ClientNow() + this.ServerMinusClientTime;
        }
        
        public long ServerFrameTime()
        {
            return this.FrameTime + this.ServerMinusClientTime;
        }
        
        public long Transition(DateTime d)
        {
            return (d.Ticks - dt.Ticks) / 10000L;
        }
        
        public long GetSecond()
        {
            return this.FrameTime / 1000L;
        }
        
        public long GetMinute()
        {
            return this.FrameTime / 1000L / 60L;
        }
        
        public long GetHour()
        {
            return this.FrameTime / 1000L / 3600L;
        }
        
        /// <summary>
        ///  获取当天
        /// </summary>
        /// <returns></returns>
        public long GetDay()
        {
            return this.FrameTime / 1000L / 3600L / 24L;
        }
        
        /// <summary>
        /// 获取当天的秒数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetSecondSinceZero(long time)
        {
            var d = this.Time(time);
            return d.Hour * 3600 + d.Minute * 60 + d.Second;
        }

        /// <summary>
        /// 获取当天零点的时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public long GetZeroSecond(long? time = null)
        {
            time ??= FrameTime;
            return (long)time - GetSecondSinceZero(time.Value);
        }
    }
}