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
        [Route(SystemConstantTwo.LeadingGroupPromotionRoute)]
        public List<LeadingDateHeaderDto> GetLeadingGroupPromotion(DateTime data)
        {
            return _replayService.GetLeadingGroupPromotion(data);
        }

        /// <summary>
        /// 获取市场天梯数据
        /// </summary>
        /// <param name="data">日期（yyyyMMdd）</param>
        [HttpPost]
        [Route(SystemConstantTwo.MarketLadderRoute)]
        public MarketLadderDto GetMarketLadder(DateTime data)
        {
            return _replayService.GetMarketLadder(data);
        }
    }
}
