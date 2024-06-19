using StockReview.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockReview.Domain.Entities
{
    /// <summary>
    /// 股票明细
    /// </summary>
    [Table("tb_stockdetail")]
    public class StockDetailEntity : EntityBase
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
        /// 涨幅
        /// </summary>
        public string ZF { get; set; }

        /// <summary>
        /// 最新
        /// </summary>
        public string Latest { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 首封时间
        /// </summary>
        public string FirstLetterTime { get; set; }

        /// <summary>
        /// 最后封板
        /// </summary>
        public string LastFBan { get; set; }

        /// <summary>
        /// 涨停形态
        /// </summary>
        public string LimitUpType { get; set; }

        /// <summary>
        /// 开板
        /// </summary>
        public string OpenNum { get; set; }

        /// <summary>
        /// 几天几板
        /// </summary>
        public string SeveralDaysBan { get; set; }

        /// <summary>
        /// 封单额
        /// </summary>
        public string FDanMoney { get; set; }

        /// <summary>
        /// 换手率
        /// </summary>
        public string TurnoverRate { get; set; }

        /// <summary>
        /// 流通值亿
        /// </summary>
        public string CurrencyMoney { get; set; }

        /// <summary>
        /// 连板
        /// </summary>
        public string LBanNum { get; set; }

    }
}
