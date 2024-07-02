using StockReview.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Client.ContentModule.Common.Model
{
    public class MarkDataModel
    {
        public List<MarkDataToInfo> MarkDataInfos { get; set; }
    }

    public class MarkDataToInfo 
    {
        public int Year { get; set; }
        public List<MarketSentimentDataDto> MarketSentimentDataDtos { get; set; }
    }
}
