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
        public MarketLadderDto GetMarketLadder(DateTime date)
        {
            return _replayService.GetMarketLadder(date);
        }

        /// <summary>
        /// 获取板块轮动数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(SystemConstantTwo.PlateRotationRoute)]
        public PlateRotationDto GetPlateRotation(DateTime date) 
        {
            return _replayService.GetPlateRotation(date);
        }

        /// <summary>
        /// 获取炸板与跌停板数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(SystemConstantTwo.ExplosiveBoardLImitDownRoute)]
        public ExplosiveBoardLImitDownDto GetExplosiveBoardLImitDown(DateTime date)
        {
            return _replayService.GetExplosiveBoardLImitDown(date);
        }

        /// <summary>
        /// 获取炸板与跌停板数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(SystemConstantTwo.DragonTigerRoute)]
        public DragonTigerDto GetDragonTiger(DateTime date)
        {
            return _replayService.GetDragonTiger(date);
        }
    }
}
