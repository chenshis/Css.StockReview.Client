using StockReview.Api.Dtos;
using System.Collections.Generic;

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
        BulletinBoardDto GetBulletinBoard(string day);

        /// <summary>
        /// 获取历史看板数据
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        BulletinBoardDto GetHisBulletinBoard(string day);

        /// <summary>
        /// 获取当天实时数据
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        EmotionDetailDto GetEmotionDetail(string day);

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        EmotionDetailDto GetHisEmotionDetail(string day);

        /// <summary>
        /// 获取当天
        /// </summary>
        /// <returns></returns>
        string GetCurrentDay();

        /// <summary>
        /// 日期初始化
        /// </summary>
        void FilterDates();

        /// <summary>
        /// 获取股票数据
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        List<StockDataDto> GetStockDatas(string stockId);

        /// <summary>
        /// 获取股票明细数据
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        StockDetailDataDto GetStockDetails(string stockId, string day);

        /// <summary>
        /// stock
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns></returns>
        StockDto GetStock(StockRequestDto request);
    }
}
