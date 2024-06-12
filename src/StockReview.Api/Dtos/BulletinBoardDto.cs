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
        /// <summary>
        /// 涨停
        /// </summary>
        public string TodayLimitUp { get; set; }
        public string YesterdayLimitUp { get; set; }
        /// <summary>
        /// 下跌
        /// </summary>
        public string TodayLimitDown { get; set; }
        public string YesterdayLimitDown { get; set; }
        /// <summary>
        /// 上涨
        /// </summary>
        public string TodayRise { get; set; }
        public string YesterdayRise { get; set; }
        /// <summary>
        /// 下跌
        /// </summary>
        public string TodayFall { get; set; }
        public string YesterdayFall { get; set; }
        /// <summary>
        /// 今日量能
        /// </summary>
        public string TodayCalorimeter { get; set; }
        /// <summary>
        /// 昨日量能
        /// </summary>
        public string YesterdayCalorimeter { get; set; }
        /// <summary>
        /// 北向资金
        /// </summary>
        public string NorthboundFunds { get; set; }
        public string SecondBoardPercent { get; set; }
        public string ThirdBoardPercent { get; set; }
        public string HighBoardPercent { get; set; }
        /// <summary>
        /// 情绪
        /// </summary>
        public string EmotionPercent { get; set; }
        public string TodayFryingRate { get; set; }
        public string YesterdayFryingRate { get; set; }
        public string CityPower { get; set; }

        /// <summary>
        /// 今日涨停破板率
        /// </summary>
        public string TodayZTPBRate { get; set; }

        /// <summary>
        /// 昨日涨停今表现
        /// </summary>
        public string YesterdayZTJBX { get; set; }

        /// <summary>
        /// 昨日连扳今表现
        /// </summary>
        public string YesterdayLBJBX { get; set; }

        /// <summary>
        /// 昨日破板今表现
        /// </summary>
        public string YesterdayPBJBX { get; set; }
    }
}
