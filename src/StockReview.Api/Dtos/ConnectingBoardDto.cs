using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
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
}
