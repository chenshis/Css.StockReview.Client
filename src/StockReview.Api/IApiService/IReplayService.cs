using Microsoft.AspNetCore.Mvc;
using StockReview.Api.Dtos;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.IApiService
{
    /// <summary>
    /// 复盘接口
    /// </summary>
    public interface IReplayService
    {
        /// <summary>
        /// 获取龙头晋级数据
        /// </summary>
        /// <param name="date"></param>
        List<LeadingDateHeaderDto> GetLeadingGroupPromotion(DateTime date);

        /// <summary>
        /// 获取市场天梯数据
        /// </summary>
        /// <param name="date"></param>
        MarketLadderDto GetMarketLadder(DateTime date);

        /// <summary>
        /// 获取市场天梯数据
        /// </summary>
        /// <param name="date"></param>
        PlateRotationDto GetPlateRotation(DateTime date);

        /// <summary>
        /// 获取炸板与跌停板数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        ExplosiveBoardLImitDownDto GetExplosiveBoardLImitDown(DateTime date);

        /// <summary>
        /// 获取炸板与跌停板数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>

        DragonTigerDto GetDragonTiger(DateTime date);
    }
}
