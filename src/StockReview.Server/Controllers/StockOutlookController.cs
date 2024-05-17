using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    //[Authorize]
    public class StockOutlookController : StockReviewControllerBase
    {
        private readonly IStockOutlookServerApiService _stockOutlookServerApiService;

        public StockOutlookController(IStockOutlookServerApiService stockOutlookServerApiService)
        {
            this._stockOutlookServerApiService = stockOutlookServerApiService;
        }

        [HttpGet]
        [Route(SystemConstant.BulletinBoardRoute)]
        public BulletinBoardDto GetBulletinBoard()
        {
            return _stockOutlookServerApiService.GetBulletinBoard();
        }
    }
}
