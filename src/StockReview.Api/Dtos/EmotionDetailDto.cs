using System.Collections.Generic;

namespace StockReview.Api.Dtos
{
    /// <summary>
    /// 情绪详情数据
    /// </summary>
    public class EmotionDetailDto
    {
        /// <summary>
        /// 柱状图
        /// </summary>
        public List<HistogramDto> histogram { get; set; }

        /// <summary>
        /// 根数据
        /// </summary>
        public Root root { get; set; }
    }

    /// <summary>
    /// 柱状图
    /// </summary>
    public class HistogramDto
    {
        public string xvalue { get; set; }

        public string yvalue { get; set; }
    }


    public class Root
    {
        public int status_code { get; set; }

        public RootData data { get; set; }

        public string status_msg { get; set; }
    }

    public class RootData
    {
        public Page page { get; set; }

        public IList<Info> info { get; set; }

        public LimitUpCount limit_up_count { get; set; }

        public LimitDownCount limit_down_count { get; set; }
    }

    public class LimitDownCount
    {
        public Today2 today { get; set; }

        public Yesterday2 yesterday { get; set; }
    }

    public class Yesterday2
    {
        public int num { get; set; }

        public int history_num { get; set; }

        public string rate { get; set; }

        public int open_num { get; set; }
    }


    public class Today2
    {
        public int num { get; set; }

        public int history_num { get; set; }

        public string rate { get; set; }

        public int open_num { get; set; }
    }

    public class LimitUpCount
    {
        public Today today { get; set; }

        public Yesterday yesterday { get; set; }
    }
    public class Today
    {
        public int num { get; set; }

        public int history_num { get; set; }

        public string rate { get; set; }

        public int open_num { get; set; }
    }

    public class Yesterday
    {
        public int num { get; set; }

        public int history_num { get; set; }

        public string rate { get; set; }

        public int open_num { get; set; }
    }

    public class Info
    {
        public int? open_num { get; set; }

        public string first_limit_up_time { get; set; }

        public string last_limit_up_time { get; set; }

        public string code { get; set; }

        public string limit_up_type { get; set; }

        public string currency_value { get; set; }

        public string change_rate { get; set; }

        public string turnover_rate { get; set; }

        public string reason_type { get; set; }

        public string order_amount { get; set; }

        public string high_days { get; set; }

        public string name { get; set; }

        public string latest { get; set; }
    }

    public class Page
    {
        public int limit { get; set; }

        public int total { get; set; }

        public int count { get; set; }

        public int page { get; set; }
    }
}
