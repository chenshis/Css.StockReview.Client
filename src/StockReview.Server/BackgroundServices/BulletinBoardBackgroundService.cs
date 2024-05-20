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
                if (bulletinBoard == null)
                {
                    return Task.CompletedTask;
                }

                memoryCache.Set(SystemConstant.BulletinBoardKey, bulletinBoard);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private FormUrlEncodedContent GetFormUrlEncodedContent(string a,
                                                             string c = SystemConstant.HomeDingPan,
                                                             string apiv = SystemConstant.apivW31,
                                                             string phoneOSNew = SystemConstant.PhoneOSNew,
                                                             string deviceID = SystemConstant.DeviceID,
                                                             string verSion = SystemConstant.VerSion57012,
                                                             string Day = null)
        {
            Dictionary<string, string> parameters = new()
            {
                [nameof(a)] = a,
                [nameof(c)] = c,
                [nameof(apiv)] = apiv,
                [nameof(phoneOSNew)] = phoneOSNew,
                [nameof(deviceID)] = deviceID,
                [nameof(verSion)] = verSion
            };

            if (!string.IsNullOrWhiteSpace(Day))
            {
                parameters[nameof(Day)] = Day;
            }

            var content = new FormUrlEncodedContent(parameters);
            return content;
        }


    }
}
