using System.Collections.Generic;

namespace StockReview.Api.Dtos
{
    /// <summary>
    /// 连板晋级
    /// </summary>
    public class ConnectingBoardDto
    {
        /// <summary>
        /// 1 2 3 4 5
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 今天
        /// </summary>
        public List<ConnectingBoardTodayDto> ConnectingBoardTodays { get; set; }

        /// <summary>
        /// 昨天
        /// </summary>
        public List<ConnectingBoardYesterdayDto> ConnectingBoardYesterdays { get; set; }
    }

    /// <summary>
    /// 今天连板晋级数据
    /// </summary>
    public class ConnectingBoardTodayDto
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 涨停时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 封单
        /// </summary>
        public string Closing { get; set; }
        /// <summary>
        /// 最大封单
        /// </summary>
        public string MaxClosing { get; set; }
        /// <summary>
        /// 主力净额
        /// </summary>
        public string MainForceNet { get; set; }
        /// <summary>
        /// 主力买入
        /// </summary>
        public string MainBuy { get; set; }
        /// <summary>
        /// 主力卖出
        /// </summary>
        public string MainSell { get; set; }
        /// <summary>
        /// 成交额
        /// </summary>
        public string TransactionAmount { get; set; }
        /// <summary>
        /// 板块
        /// </summary>
        public string Plate { get; set; }
        /// <summary>
        /// 振幅
        /// </summary>
        public string Amplitude { get; set; }
        /// <summary>
        /// 实际流通
        /// </summary>
        public string ActualCirculation { get; set; }
        /// <summary>
        /// 实际换手
        /// </summary>
        public string ActualTurnover { get; set; }
    }

    /// <summary>
    /// 昨天连板晋级数据
    /// </summary>
    public class ConnectingBoardYesterdayDto
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public string ZhangF { get; set; }
        /// <summary>
        /// 板块
        /// </summary>
        public string Plate { get; set; }
        /// <summary>
        /// 振幅
        /// </summary>
        public string Amplitude { get; set; }
        /// <summary>
        /// 主力净额
        /// </summary>
        public string MainForceNet { get; set; }
        /// <summary>
        /// 主力买入
        /// </summary>
        public string MainBuy { get; set; }
        /// <summary>
        /// 主力卖出
        /// </summary>
        public string MainSell { get; set; }
        /// <summary>
        /// 成交额
        /// </summary>
        public string TransactionAmount { get; set; }
        /// <summary>
        /// 实际流通
        /// </summary>
        public string ActualCirculation { get; set; }
        /// <summary>
        /// 实际换手
        /// </summary>
        public string ActualTurnover { get; set; }
    }
}
