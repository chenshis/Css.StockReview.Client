using System.Collections.Generic;

namespace StockReview.Api.Dtos
{
    /// <summary>
    /// 情绪详情连板数据传输对象
    /// </summary>
    public class EmotionDetailLBanDto
    {
        public class Code_listItem
        {
            public string code { get; set; }

            public string name { get; set; }

            public int market_id { get; set; }

            public int continue_num { get; set; }
        }

        public class DataItem
        {
            public int height { get; set; }

            public List<Code_listItem> code_list { get; set; }

            public int number { get; set; }
        }

        public class Root
        {
            public int status_code { get; set; }

            public List<DataItem> data { get; set; }

            public string status_msg { get; set; }
        }
    }
}
