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
        /// 板块轮动数据地址
        /// trade_date  日期
        /// </summary>                              
        public const string PlateRotationPostDataUrl = "https://apphis.longhuvip.com/w1/api/index.php";
        /// <summary>
        /// 板块轮动数据地址
        /// trade_date  日期
        /// </summary>                              
        public const string PlateRotationPostDayDataUrl = " https://apphq.longhuvip.com/w1/api/index.php";
        /// <summary>
        /// 炸板与跌停板数据地址
        /// trade_date  日期
        /// </summary>                              
        public const string ExplosivePostDataUrl = "https://flash-api.xuangubao.cn/api/pool/detail";
        /// <summary>
        /// 炸板与跌停板数据地址
        /// trade_date  日期
        /// </summary>                              
        public const string ExplosivePostDayDataUrl = "https://flash-api.xuangubao.cn/api/pool/detail";
                                                  
        
        /// <summary>
        /// 龙头晋级数据路由
        /// </summary>
        public const string LeadingGroupPromotionRoute = "v1/stockreview/leading/list";
        /// <summary>
        /// 市场天梯数据路由
        /// </summary>
        public const string MarketLadderRoute = "v1/stockreview/market/list";
        /// <summary>
        /// 板块轮动数据路由
        /// </summary>
        public const string PlateRotationRoute = "v1/stockreview/plate/list";
        /// <summary>
        /// 炸板与跌停板数据路由
        /// </summary>
        public const string ExplosiveBoardLImitDownRoute = "v1/stockreview/explosive/list";
        /// <summary>
        /// 龙虎榜数据路由
        /// </summary>
        public const string DragonTigerRoute = "v1/stockreview/dragon/list";
    }
}
