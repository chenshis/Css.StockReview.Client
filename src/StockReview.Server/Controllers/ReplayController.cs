using Microsoft.AspNetCore.Mvc;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;

namespace StockReview.Server.Controllers
{
    /// <summary>
    /// 复盘管理
    /// </summary>
    public class ReplayController : StockReviewControllerBase
    {
        private readonly IReplayService _replayService;

        public ReplayController(IReplayService replayService)
        {
            this._replayService = replayService;
        }

        /// <summary>
        /// 获取龙头晋级数据
        /// </summary>
        /// <param name="data">日期（yyyyMMdd）</param>
        [HttpPost]
        [Route(SystemConstant.LeadingGroupPromotionRoute)]
        public List<LeadingDateHeaderDao> GetLeadingGroupPromotion(DateTime data)
        {
            var leadingList = _replayService.GetLeadingGroupPromotion(data);

            return leadingList;
        }
    }
}
