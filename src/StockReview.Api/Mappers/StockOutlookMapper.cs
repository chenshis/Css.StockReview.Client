using StockReview.Api.Dtos;
using StockReview.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Mappers
{
    public static class StockOutlookMapper
    {
        /// <summary>
        /// 实体
        /// </summary>
        /// <param name="boardDto"></param>
        /// <returns></returns>
        public static BulletinBoardEntity ToEntity(this BulletinBoardDto boardDto)
        {
            if (boardDto == null) return null;

            BulletinBoardEntity boardEntity = new();
            boardEntity.TodayLimitUp = boardDto.TodayLimitUp;
            boardEntity.YesterdayLimitUp = boardDto.YesterdayLimitUp;
            boardEntity.TodayLimitDown = boardDto.TodayLimitDown;
            boardEntity.YesterdayLimitDown = boardDto.YesterdayLimitDown;
            boardEntity.TodayRise = boardDto.TodayRise;
            boardEntity.YesterdayRise = boardDto.YesterdayRise;
            boardEntity.TodayFall = boardDto.TodayFall;
            boardEntity.YesterdayFall = boardDto.YesterdayFall;
            boardEntity.TodayCalorimeter = boardDto.TodayCalorimeter;
            boardEntity.YesterdayCalorimeter = boardDto.YesterdayCalorimeter;
            boardEntity.NorthboundFunds = boardDto.NorthboundFunds;
            boardEntity.SecondBoardPercent = boardDto.SecondBoardPercent;
            boardEntity.ThirdBoardPercent = boardDto.ThirdBoardPercent;
            boardEntity.HighBoardPercent = boardDto.HighBoardPercent;
            boardEntity.EmotionPercent = boardDto.EmotionPercent;
            boardEntity.TodayFryingRate = boardDto.TodayFryingRate;
            boardEntity.YesterdayFryingRate = boardDto.YesterdayFryingRate;
            boardEntity.CityPower = boardDto.CityPower;
            boardEntity.TodayZTPBRate = boardDto.TodayZTPBRate;
            boardEntity.YesterdayZTJBX = boardDto.YesterdayZTJBX;
            boardEntity.YesterdayLBJBX = boardDto.YesterdayLBJBX;
            boardEntity.YesterdayPBJBX = boardDto.YesterdayPBJBX;


            return boardEntity;
        }

        /// <summary>
        /// 实体
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static List<StockDetailEntity> ToEntities(this IList<Info> infos)
        {
            List<StockDetailEntity> stockDetailEntities = new List<StockDetailEntity>();
            if (infos != null)
            {
                foreach (var item in infos)
                {
                    StockDetailEntity stockDetail = new();
                    stockDetail.StockId = item.code;
                    stockDetail.StockName = item.name;
                    stockDetail.ZF = Convert.ToDouble(item.change_rate).ToString("0.00");
                    stockDetail.Latest = item.latest;
                    stockDetail.Reason = item.reason_type;
                    stockDetail.FirstLetterTime = GetDateTime(item.first_limit_up_time).ToString("HH:mm:ss");
                    stockDetail.LastFBan = GetDateTime(item.last_limit_up_time).ToString("HH:mm:ss");
                    stockDetail.LimitUpType = item.limit_up_type;
                    stockDetail.OpenNum = item.open_num?.ToString();
                    stockDetail.SeveralDaysBan = item.high_days;
                    stockDetail.FDanMoney = zWy(Convert.ToDouble(item.order_amount)).ToString();
                    stockDetail.TurnoverRate = Convert.ToDouble(item.turnover_rate).ToString("F2") + "%";
                    stockDetail.CurrencyMoney = item.currency_value != null ? zEy(Convert.ToDouble(item.currency_value)).ToString() : null;
                    stockDetail.LBanNum = item.LBanNum;

                    stockDetailEntities.Add(stockDetail);
                }
            }
            return stockDetailEntities;
        }

        /// <summary>
        /// 实体
        /// </summary>
        /// <param name="stockDetail"></param>
        /// <param name="stockId"></param>
        /// <param name="stockName"></param>
        /// <returns></returns>
        public static List<TimeIndexChartEntity> ToEntities(this StockDetailDataDto stockDetail, string stockId, string stockName)
        {
            List<TimeIndexChartEntity> timeIndexChartEntities = new();
            if (stockDetail != null)
            {
                var totalCount = stockDetail.Times.Count();
                for (int i = 0; i < totalCount; i++)
                {
                    TimeIndexChartEntity timeIndexChartEntity = new();
                    timeIndexChartEntity.StockId = stockId;
                    timeIndexChartEntity.StockName = stockName;
                    timeIndexChartEntity.Time = stockDetail.Times[i];
                    timeIndexChartEntity.Latest = stockDetail.Latests[i].ToString();
                    timeIndexChartEntity.Avg = stockDetail.Avgs[i].ToString();
                    timeIndexChartEntity.Turnover = stockDetail.Turnovers[i].ToString();
                    timeIndexChartEntity.Volume = stockDetail.Volumes[i].ToString();

                    timeIndexChartEntities.Add(timeIndexChartEntity);
                }
            }

            return timeIndexChartEntities;
        }

        #region 私有方法

        private static DateTime GetDateTime(string strLongTime)
        {
            long num = Convert.ToInt64(strLongTime) * 10000000L;
            long ticks = new DateTime(1970, 1, 1, 8, 0, 0).Ticks + num;
            return new DateTime(ticks);
        }
        private static double zWy(double e)
        {
            return Math.Round(e * 0.0001, 0);
        }
        private static double zEy(double e)
        {
            return Math.Round(e * 1E-08, 2);
        }

        #endregion
    }
}
