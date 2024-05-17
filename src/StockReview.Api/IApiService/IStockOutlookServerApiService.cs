using StockReview.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.IApiService
{
    /// <summary>
    /// 看盘服务接口
    /// </summary>
    public interface IStockOutlookServerApiService
    {
        /// <summary>
        /// 获取看板信息
        /// </summary>
        BulletinBoardDto GetBulletinBoard();
    }
}
