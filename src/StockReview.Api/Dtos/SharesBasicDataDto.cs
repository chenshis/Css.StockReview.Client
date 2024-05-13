using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class SharesBasicDataDto
    {
        public class Limit_up_listItem
        {
            public string first_limit_up_time { get; set; }

            public string limit_up_reason { get; set; }

            public string stockCode { get; set; }

            public string stockName { get; set; }
        }

        public class Root
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public string trade_date { get; set; }

            public Total total { get; set; }

            /// <summary>
            /// 首板
            /// </summary>
            public First first { get; set; }
            /// <summary>
            /// 二板
            /// </summary>
            public Second second { get; set; }
            /// <summary>
            /// 三板
            /// </summary>
            public Third third { get; set; }
            /// <summary>
            /// 四板
            /// </summary>
            public Fourth fourth { get; set; }
            /// <summary>
            /// 五板
            /// </summary>
            public Fifth fifth { get; set; }
            /// <summary>
            /// 最高板
            /// </summary>
            public More more { get; set; }
        }
        public class Fifth
        {
            public List<Limit_up_listItem> limit_up_list { get; set; }

            public int limit_up_num { get; set; }

            public string promotion_rate { get; set; }

            public string plate_frying_rate { get; set; }

            public string call_auction_rise { get; set; }
        }

        public class More
        {
            public List<Limit_up_listItem> limit_up_list { get; set; }

            public int limit_up_num { get; set; }

            public string promotion_rate { get; set; }

            public string plate_frying_rate { get; set; }

            public string call_auction_rise { get; set; }
        }

        public class Fourth
        {
            public List<Limit_up_listItem> limit_up_list { get; set; }

            public int limit_up_num { get; set; }

            public string promotion_rate { get; set; }

            public string plate_frying_rate { get; set; }

            public string call_auction_rise { get; set; }
        }

        public class Third
        {
            public List<Limit_up_listItem> limit_up_list { get; set; }

            public int limit_up_num { get; set; }

            public string promotion_rate { get; set; }

            public string plate_frying_rate { get; set; }

            public string call_auction_rise { get; set; }
        }

        public class Second
        {
            public List<Limit_up_listItem> limit_up_list { get; set; }

            public int limit_up_num { get; set; }

            public string promotion_rate { get; set; }

            public string plate_frying_rate { get; set; }

            public string call_auction_rise { get; set; }
        }

        public class Total
        {
            public int limit_up_num { get; set; }

            public string promotion_rate { get; set; }

            public string plate_frying_rate { get; set; }

            public string call_auction_rise { get; set; }
        }

        public class First
        {
            public List<Limit_up_listItem> limit_up_list { get; set; }

            public int limit_up_num { get; set; }

            public string promotion_rate { get; set; }

            public string plate_frying_rate { get; set; }

            public string call_auction_rise { get; set; }

            public string befor_promotion_rate { get; set; }
        }
    }
}
