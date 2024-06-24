using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;

namespace StockReview.Server.BackgroundServices
{
    /// <summary>
    /// 股票数据后台服务
    /// </summary>
    public class StockDataBackgroundService : StockReviewBackgroundService
    {
        public StockDataBackgroundService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Schedule = "0 0 0/2 * * *";
        }

        protected override string Schedule { get; set; }

        protected override Task Process(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<IStockOutlookServerApiService>().SaveStock();
            return Task.CompletedTask;
        }
    }
}
