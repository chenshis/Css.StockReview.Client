using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Infrastructure.Config
{
    /// <summary>
    /// 系统常量 ，防止冲突，最后合并至SystemConstant
    /// </summary>
    public static class SystemConstantTwo
    {
        /// <summary>
        /// 龙头数据地址
        /// trade_date  日期
        /// </summary>
        public const string LeadingGroupPromotionDataUrl = "https://extquota-h.10jqka.com.cn/noauth/dxql/astock/continuezt/v1/share?trade_date=";

        /// <summary>
        /// 市场天梯数据地址
        /// trade_date  日期
        /// </summary>
        public const string MarketLadderDataUrl = "https://flash-api.xuangubao.cn/api/surge_stock/plates?date=";



        /// <summary>
        /// 龙头晋级数据路由
        /// </summary>
        public const string LeadingGroupPromotionRoute = "v1/stockreview/leading/list";

        /// <summary>
        /// 市场天梯数据路由
        /// </summary>
        public const string MarketLadderRoute = "v1/stockreview/market/list";
    }
}
