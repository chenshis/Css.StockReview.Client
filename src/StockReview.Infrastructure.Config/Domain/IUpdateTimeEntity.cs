using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Infrastructure.Config.Domain
{
    /// <summary>
    /// 修改时间
    /// </summary>
    public interface IUpdateTimeEntity
    {
        DateTime UpdateTime { get; set; }
    }
}