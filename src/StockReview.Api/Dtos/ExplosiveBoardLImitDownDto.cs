using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class ExplosiveBoardLImitDownDto
    {
        public List<ExplosiveFriedIndividualInfo> ExplosiveFriedIndividualInfos { get; set; } 
        public List<ExplosiveLimitUpStaticsInfo> ExplosiveLimitUpStaticsInfos { get; set; }
        public List<ExplosiveLimitDownStaticsInfo> ExplosiveLimitDownStaticsInfos { get; set; }
        public List<ExplosiveYeasterdayLimitUpStaticsInfo> ExplosiveYeasterdayLimitUpStaticsInfos { get; set; } 
    }

    public class ExplosiveYeasterdayLimitUpStaticsInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string ExpYeaCode { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string ExpYeaName { get; set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public double ExpYeaIncrease { get; set; }
        /// <summary>
        /// 昨连板
        /// </summary>
        public int ExpYeaConsecutiveBoard { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string ExpYeaModule { get; set; }
        /// <summary>
        /// 变化
        /// </summary>
        public string ExpYeaChange { get; set; }
    }

    public class ExplosiveLimitDownStaticsInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 股票编码
        /// </summary>
        public string ExpDownCode { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string ExpDownName { get; set; }
        /// <summary>
        /// 首封时间
        /// </summary>
        public string ExpDownFirstSealingTime { get; set; }
        /// <summary>
        /// 尾封时间
        /// </summary>
        public string ExpDownTailSealingTime { get; set; }
        /// <summary>
        /// 开板
        /// </summary>
        public int ExpDownStartDoingBusiness { get; set; }
        /// <summary>
        /// 连板
        /// </summary>
        public int ExpDownStartConnectedBoard { get; set; }
    }

    public class ExplosiveLimitUpStaticsInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 板块名称
        /// </summary>
        public string ExpLimitName { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int ExpLimitTotal { get; set; }
        /// <summary>
        /// 上涨
        /// </summary>
        public int ExpLimitUp { get; set; }
        /// <summary>
        /// 下跌
        /// </summary>
        public int ExpLimitDown { get; set; }

    }

    public class ExplosiveFriedIndividualInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 股票编码
        /// </summary>
        public string ExpSharesCode { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string ExpSharesName { get; set; }

        /// <summary>
        /// 板块概念
        /// </summary>
        public string ExpSharesConcept { get; set; }

        /// <summary>
        /// 首封时间
        /// </summary>
        public string ExpSharesFirstSealingTime { get; set; }

        /// <summary>
        /// 尾封时间
        /// </summary>
        public string ExpSharesTailSealingTime { get; set; }

        /// <summary>
        /// 最后炸板时间
        /// </summary>
        public string ExpSharesLastFryingTime { get; set; }

        /// <summary>
        /// 开板
        /// </summary>
        public int ExpSharesStartDoingBusiness { get; set; }

        /// <summary>
        /// 连板
        /// </summary>
        public int ExpSharesStartConnectedBoard { get; set; }

        /// <summary>
        /// 流通市值
        /// </summary>
        public double ExpSharesCirculatingMarketValue { get; set; }
    }
}
