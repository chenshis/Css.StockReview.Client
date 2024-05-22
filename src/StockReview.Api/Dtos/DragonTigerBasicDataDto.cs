using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class DragonTigerBasicDataDto
    {
        public class ColDesItem
        {
            public string Name { get; set; }
        }

        public class ResultSetsItem
        {
            public List<ColDesItem> ColDes { get; set; }

            public List<List<string>> Content { get; set; }
        }

        public class Root
        {

            public int ErrorCode { get; set; }

            public List<ResultSetsItem> ResultSets { get; set; }
        }
    }
}
