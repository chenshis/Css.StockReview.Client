using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class MarketSentimentDataDto
    {
        /// <summary>
        /// 1.最高板
        /// 2.炸板
        /// 3.涨停
        /// 4.跌停
        /// </summary>
        public int type { get; set; }
        public string date { get; set; }
        public List<string> name { get; set; }
    }
}
