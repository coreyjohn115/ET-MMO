using System;

namespace ET
{
    public partial class Language
    {
        public string Fmt(params object[] args)
        {
            return string.Format(this.Msg, args);
        }

        public byte[] ColorBytes = new byte[4];

        public override void EndInit()
        {
            var colorCfg = CilentColorCategory.Instance.Get2(this.Color);
            if (colorCfg == default)
            {
                return;
            }

            int colorValue = 0;
            try
            {
                colorValue = Convert.ToInt32(colorCfg.Color, 0x10);
            }
            catch (Exception e)
            {
                Log.Error(new Exception($"invalid hexColorStr: {this.Color}", e));
                return;
            }

            var b = (byte)(colorValue & 0xff);
            var g = (byte)(colorValue >> 8 & 0xff);
            var r = (byte)(colorValue >> 16 & 0xff);
            ColorBytes[0] = r;
            ColorBytes[1] = g;
            ColorBytes[2] = b;
            ColorBytes[3] = byte.MaxValue;
        }
    }
}