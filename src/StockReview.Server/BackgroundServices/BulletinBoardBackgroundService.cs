using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using StockReview.Infrastructure.Config;
using StockReview.Api.Dtos;
using Microsoft.Extensions.Caching.Memory;
using StockReview.Api.IApiService;

namespace StockReview.Server.BackgroundServices
{
    public class BulletinBoardBackgroundService : StockReviewBackgroundService
    {
        public BulletinBoardBackgroundService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Schedule = "0/2 * * * * *";
        }

        protected override string Schedule { get; set; }

        protected override Task Process(IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
            var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
            var stockReviewApiService = serviceProvider.GetRequiredService<IStockOutlookServerApiService>();
            try
            {
                var day = memoryCache.Get<string>(SystemConstant.StockSelectedDayKey);
                if (day == null)
                {
                    day = stockReviewApiService.GetCurrentDay();
                }
                var bulletinBoard = stockReviewApiService.GetHisBulletinBoard(day);
                if (bulletinBoard != null)
                {
                    memoryCache.Set(SystemConstant.BulletinBoardKey, bulletinBoard);
                }

                var emotionDetail = stockReviewApiService.GetHisEmotionDetail(day);
                if (emotionDetail != null)
                {
                    memoryCache.Set(SystemConstant.EmotionDetailKey, emotionDetail);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

    }
}
