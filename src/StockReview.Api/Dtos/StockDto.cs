using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class StockDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<StockDataDto> StockDatas { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<StockDetailDataDto> StockDetailDatas { get; set; }
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
        public string Time { get; set; }
        public double Latest { get; set; }
        public double Avg { get; set; }
        public double Volume { get; set; }
    }

}
