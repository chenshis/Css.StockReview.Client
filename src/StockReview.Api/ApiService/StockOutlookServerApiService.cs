using Microsoft.Extensions.Logging;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace StockReview.Api.ApiService
{
    /// <summary>
    /// Stock Out Look Implementation
    /// </summary>
    public class StockOutlookServerApiService : IStockOutlookServerApiService
    {
        private readonly ILogger<StockOutlookServerApiService> _logger;

        public StockOutlookServerApiService(ILogger<StockOutlookServerApiService> logger)
        {
            this._logger = logger;
        }

        public BulletinBoardDto GetBulletinBoard()
        {

            return new BulletinBoardDto();
        }


    }
}
