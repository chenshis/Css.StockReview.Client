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
        /// 龙头晋级数据地址
        /// trade_date  日期
        /// </summary>
        public const string LeadingGroupPromotionDataUrl = "https://extquota-h.10jqka.com.cn/noauth/dxql/astock/continuezt/v1/share?trade_date=";

      
    }
}
