﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config;

namespace StockReview.Server.Controllers
{
    /// <summary>
    /// 看盘
    /// </summary>
    public class StockOutlookController : StockReviewControllerBase
    {
        private readonly IStockOutlookServerApiService _stockOutlookServerApiService;

        public StockOutlookController(IStockOutlookServerApiService stockOutlookServerApiService)
        {
            this._stockOutlookServerApiService = stockOutlookServerApiService;
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleEnum.Ordinary)},{nameof(RoleEnum.VIP)},{nameof(RoleEnum.Admin)}")]
        [Route(SystemConstant.BulletinBoardRoute)]
        public BulletinBoardDto GetBulletinBoard([FromBody] string day)
        {
            return _stockOutlookServerApiService.GetBulletinBoard(day);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleEnum.Ordinary)},{nameof(RoleEnum.VIP)},{nameof(RoleEnum.Admin)}")]
        [Route(SystemConstant.EmotionDetailRoute)]
        public EmotionDetailDto GetEmotionDetail([FromBody] string day)
        {
            return _stockOutlookServerApiService.GetEmotionDetail(day);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleEnum.Ordinary)},{nameof(RoleEnum.VIP)},{nameof(RoleEnum.Admin)}")]
        [Route(SystemConstant.TodayRoute)]
        public string GetSelectedDate()
        {
            return _stockOutlookServerApiService.GetCurrentDay();
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleEnum.Ordinary)},{nameof(RoleEnum.VIP)},{nameof(RoleEnum.Admin)}")]
        [Route(SystemConstant.StockDataRoute)]
        public StockDto PostStocks([FromBody] StockRequestDto request)
        {
            return _stockOutlookServerApiService.GetStock(request);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleEnum.Ordinary)},{nameof(RoleEnum.VIP)},{nameof(RoleEnum.Admin)}")]
        [Route(SystemConstant.StockConnectingBoard)]
        public List<ConnectingBoardDto> ConnectingBoard([FromBody] string day)
        {
            return _stockOutlookServerApiService.GetConnectingBoard(day);
        }
    }
}
