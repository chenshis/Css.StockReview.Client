using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class ZaBanBasicDataDto
    {
        public class ItemsItem
        {
            public int timestamp { get; set; }

            public int status { get; set; }
        }

        public class Limit_timeline
        {
            public List<ItemsItem> items { get; set; }
        }

        public class Related_platesItem
        {
            public string plate_name { get; set; }

            public string plate_reason { get; set; }
        }

        public class Surge_reason
        {
            public string symbol { get; set; }

            public string stock_reason { get; set; }

            public List<Related_platesItem> related_plates { get; set; }
        }

        public class DataItem
        {
            public string break_limit_down_times { get; set; }

            public int break_limit_up_times { get; set; }

            public double buy_lock_volume_ratio { get; set; }

            public double change_percent { get; set; }

            public string first_break_limit_down { get; set; }

            public string first_break_limit_up { get; set; }

            public string first_limit_down { get; set; }

            public int first_limit_up { get; set; }

            public string is_new_stock { get; set; }

            public double issue_price { get; set; }

            public string last_break_limit_down { get; set; }

            public int last_break_limit_up { get; set; }

            public string last_limit_down { get; set; }

            public int last_limit_up { get; set; }

            public string limit_down_days { get; set; }

            public Limit_timeline limit_timeline { get; set; }

            public string limit_up_days { get; set; }

            public string listed_date { get; set; }

            public string m_days_n_boards_boards { get; set; }

            public string m_days_n_boards_days { get; set; }

            public double mtm { get; set; }

            public string nearly_new_acc_pcp { get; set; }

            public string nearly_new_break_days { get; set; }

            public double new_stock_acc_pcp { get; set; }

            public string new_stock_break_limit_up { get; set; }

            public string new_stock_limit_up_days { get; set; }

            public string new_stock_limit_up_price_before_broken { get; set; }

            public double non_restricted_capital { get; set; }

            public double price { get; set; }

            public double sell_lock_volume_ratio { get; set; }

            public string stock_chi_name { get; set; }

            public Surge_reason surge_reason { get; set; }

            public string symbol { get; set; }

            public double total_capital { get; set; }

            public double turnover_ratio { get; set; }

            public double volume_bias_ratio { get; set; }

            public string yesterday_break_limit_up_times { get; set; }

            public string yesterday_first_limit_up { get; set; }

            public string yesterday_last_limit_up { get; set; }

            public string yesterday_limit_down_days { get; set; }

            public int yesterday_limit_up_days { get; set; }
        }

        public class Root
        {

            public int code { get; set; }

            public string message { get; set; }

            public List<DataItem> data { get; set; }
        }
    }
}
