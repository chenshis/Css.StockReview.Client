using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace StockReview.Infrastructure.Config
{
    /// <summary>
    /// 随机数帮助类
    /// </summary>
    public static class RandomHelper
    {
        private static Random random = new Random();

        public static int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public static Color NextColor()
        {
            return Color.FromArgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        }
    }
}
