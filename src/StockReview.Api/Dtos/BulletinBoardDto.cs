using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    /// <summary>
    /// 看板模型数据传输对象
    /// </summary>
    public class BulletinBoardDto
    {
        public double TodayLimitUp { get; set; }

        public double YesterdayLimitUp { get; set; }

        public double TodayLimitDown { get; set; }

        public double YesterdayLimitDown { get; set; }

        public double TodayRise { get; set; }

        public double YesterdayRise { get; set; }

        public double TodayFall { get; set; }

        public double YesterdayFall { get; set; }

    }
}
