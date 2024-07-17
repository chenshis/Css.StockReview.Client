using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class MarketSentimentDataDto
    {
        public int type { get; set; }
        public string date { get; set; }
        public List<string> name { get; set; }
    }
}
