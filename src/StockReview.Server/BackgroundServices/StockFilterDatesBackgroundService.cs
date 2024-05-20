using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;

namespace StockReview.Server.BackgroundServices
{
    public class StockFilterDatesBackgroundService : StockReviewBackgroundService
    {
        public StockFilterDatesBackgroundService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Schedule = "0 0/2 * * * *";
        }

        protected override string Schedule { get; set; }

        protected override Task Process(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<IStockOutlookServerApiService>().FilterDates();
            return Task.CompletedTask;
        }
    }
}
