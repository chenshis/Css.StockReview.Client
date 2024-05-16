using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class PlateBasicDataDto
    {
        public class Root
        {
            public string jxaa = "";

            public List<List<string>> list { get; set; }

            public int Time { get; set; }

            public int Count { get; set; }

            public List<string> Day { get; set; }

            public string Min { get; set; }

            public string Max { get; set; }

            public double ttag { get; set; }

            public string errcode { get; set; }
        }
    }
}
