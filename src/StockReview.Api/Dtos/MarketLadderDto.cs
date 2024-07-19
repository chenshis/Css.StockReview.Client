using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class MarketLadderDto
    {
        public string MarketTitle { get; set; }

        public List<MarketLadderList> MarketLadderLists { get; set; }

        public List<MarketLadderNewsList> MarketLadderNewsLists { get; set; }
    }
    public class MarketLadderNewsList
    {
        /// <summary>
        /// 新闻名称
        /// </summary>
        public string MarketNewsTitle { get; set; }

        /// <summary>
        /// 新闻类型
        /// </summary>
        public string MarketNewsType { get; set; }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public string MarketNewsTitleFontColor { get; set; }

        public string MarketColor { get; set; }
    }
    public class MarketLadderList
    {
        /// <summary>
        /// 连板
        /// </summary>
        public string MarketLadderBoard { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string MarketLadderNumber { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string MarketLadderDescibe { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public List<MarketLadderInfo> MarketLadderInfos { get; set; }
    }

    public class MarketLadderToInfo 
    {
        /// <summary>
        /// 连板
        /// </summary>
        public string MarketLadderBoard { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public string MarketLadderNumber { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string MarketLadderDescibe { get; set; }
    }

    public class MarketLadderInfo
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string MarketLadderCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string MarketLadderName { get; set; }

        /// <summary>
        /// 首次涨停
        /// </summary>
        public string MarketLadderFirstLimitUp { get; set; }

        /// <summary>
        /// 涨停原因
        /// </summary>
        public string MarketLadderReasonLimitUp { get; set; }
    }
}
