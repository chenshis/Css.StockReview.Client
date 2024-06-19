using StockReview.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Domain.Entities
{
    /// <summary>
    /// 看板
    /// </summary>
    [Table("tb_bulletinboard")]
    public class BulletinBoardEntity : EntityBase
    {
        /// <summary>
        /// 今日涨停
        /// </summary>
        public string TodayLimitUp { get; set; }

        /// <summary>
        /// 昨日涨停
        /// </summary>
        public string YesterdayLimitUp { get; set; }

        /// <summary>
        /// 今日下跌
        /// </summary>
        public string TodayLimitDown { get; set; }

        /// <summary>
        /// 昨日下跌
        /// </summary>
        public string YesterdayLimitDown { get; set; }

        /// <summary>
        /// 今天上涨
        /// </summary>
        public string TodayRise { get; set; }

        /// <summary>
        /// 昨天上涨
        /// </summary>
        public string YesterdayRise { get; set; }

        /// <summary>
        /// 今天下跌
        /// </summary>
        public string TodayFall { get; set; }

        /// <summary>
        /// 昨天下跌
        /// </summary>
        public string YesterdayFall { get; set; }

        /// <summary>
        /// 今日能量
        /// </summary>
        public string TodayCalorimeter { get; set; }

        /// <summary>
        /// 昨日能量
        /// </summary>
        public string YesterdayCalorimeter { get; set; }

        /// <summary>
        /// 北向资金
        /// </summary>
        public string NorthboundFunds { get; set; }

        /// <summary>
        /// 二板
        /// </summary>
        public string SecondBoardPercent { get; set; }

        /// <summary>
        /// 三板
        /// </summary>
        public string ThirdBoardPercent { get; set; }

        /// <summary>
        /// 高板
        /// </summary>
        public string HighBoardPercent { get; set; }

        /// <summary>
        /// 情绪值
        /// </summary>
        public string EmotionPercent { get; set; }

        /// <summary>
        /// 今天炸板
        /// </summary>
        public string TodayFryingRate { get; set; }

        /// <summary>
        /// 昨天炸板
        /// </summary>
        public string YesterdayFryingRate { get; set; }

        /// <summary>
        /// 城市能量
        /// </summary>
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
