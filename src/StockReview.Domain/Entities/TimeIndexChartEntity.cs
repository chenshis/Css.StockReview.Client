using StockReview.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockReview.Domain.Entities
{
    /// <summary>
    /// 分时图
    /// </summary>
    [Table("tb_timeindexchart")]
    public class TimeIndexChartEntity : EntityBase
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 最新
        /// </summary>
        public string Latest { get; set; }

        /// <summary>
        /// 平均
        /// </summary>
        public string Avg { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public string Turnover { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public string Volume { get; set; }
    }
}
