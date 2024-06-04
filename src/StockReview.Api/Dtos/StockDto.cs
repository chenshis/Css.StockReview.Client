using System;
using System.Collections.Generic;

namespace StockReview.Api.Dtos
{
    /// <summary>
    /// dto
    /// </summary>
    public class StockDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// current date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// data
        /// </summary>
        public List<StockDataDto> StockDatas { get; set; }

        /// <summary>
        /// detail
        /// </summary>
        public StockDetailDataDto StockDetailData { get; set; }
    }

    public class StockDataDto
    {
        public DateTime Date { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Volume { get; set; }

        public double UpDown { get; set; }

        public double M5 { get; set; }

        public double M10 { get; set; }

        public double M20 { get; set; }

        public double M30 { get; set; }
    }

    public class StockDetailDataDto
    {
        /// <summary>
        /// 成交总量
        /// </summary>
        public long TotalTurnover { get; set; }

        /// <summary>
        /// 卡盘
        /// </summary>
        public double OpenPrice { get; set; }
        /// <summary>
        /// 收盘
        /// </summary>
        public double ClosePrice { get; set; }
        /// <summary>
        /// 高位
        /// </summary>
        public double HighPrice { get; set; }

        /// <summary>
        /// 低位
        /// </summary>
        public double LowPrice { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public List<double> Volumes { get; set; } = new List<double>();

        public List<string> Times { get; set; } = new List<string>();

        public List<StockDetailColorDto> Colors { get; set; } = new List<StockDetailColorDto>();

        public List<double> Latests { get; set; } = new List<double>();

        public List<double> Avgs { get; set; } = new List<double>();

        public List<double> Turnovers { get; set; } = new List<double>();
    }


    public class StockDetailColorDto
    {
        public StockDetailColorDto(double turnover, ColorEnum color)
        {
            Turnover = turnover;
            Color = color;
        }
        public double Turnover { get; set; }

        public ColorEnum Color { get; set; }
    }

    public enum ColorEnum
    {
        Red,
        Green,
        Gray
    }

}
