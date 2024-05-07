using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Infrastructure.Config.Domain
{
    /// <summary>
    /// 软删除 
    /// </summary>
    public interface ISoftDeletable
    {
        int Status { get; set; }
    }
}
