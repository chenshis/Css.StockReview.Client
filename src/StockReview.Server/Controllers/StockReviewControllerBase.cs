using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StockReview.Server.Controllers
{
    /// <summary>
    /// 基类
    /// </summary>
    [ApiWrappedFilter]
    public class StockReviewControllerBase : ControllerBase
    {
    }
}
