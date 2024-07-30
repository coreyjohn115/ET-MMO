using System;

namespace ET
{
    public partial class QualityConfig
    {
        public byte[] ColorBytes = new byte[4];

        public override void EndInit()
        {
            int colorValue = 0;
            try
            {
                colorValue = Convert.ToInt32(this.ItemNameColor, 0x10);
            }
            catch (Exception e)
            {
                Log.Error(new Exception($"invalid hexColorStr: {this.ItemNameColor}", e));
                return;
            }

            var b = (byte) (colorValue & 0xff);
            var g = (byte) (colorValue >> 8 & 0xff);
            var r = (byte) (colorValue >> 16 & 0xff);
            ColorBytes[0] = r;
            ColorBytes[1] = g;
            ColorBytes[2] = b;
            ColorBytes[3] = byte.MaxValue;
        }
    }
}