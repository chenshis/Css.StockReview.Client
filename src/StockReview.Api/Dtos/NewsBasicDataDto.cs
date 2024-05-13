using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StockReview.Api.Dtos.SharesBasicDataDto;

namespace StockReview.Api.Dtos
{
    public class NewsBasicDataDto
    {

        public class Data
        {
            public List<ItemsItem> items { get; set; }

            public int manual_updated_at { get; set; }

            public int timestamp { get; set; }
        }
        public class Root
        {
            public int code { get; set; }

            public string message { get; set; }

            public Data data { get; set; }
        }
        public class ItemsItem
        {
            public int id { get; set; }

            public string name { get; set; }

            public string description { get; set; }
        }
    }
}
