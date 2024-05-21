using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using StockReview.Infrastructure.Config.HttpClients;

namespace StockReview.Api.ApiService
{
    public class StockOutlookApiService : IStockOutlookApiService
    {
        private readonly StockHttpClient _stockHttpClient;

        public StockOutlookApiService(StockHttpClient stockHttpClient)
        {
            this._stockHttpClient = stockHttpClient;
        }
        public ApiResponse<BulletinBoardDto> GetBulletinBoard(string day)
        {
            return _stockHttpClient.Post<string, BulletinBoardDto>(SystemConstant.BulletinBoardRoute, day);
        }

        public ApiResponse<EmotionDetailDto> GetEmotionDetail(string day)
        {
            return _stockHttpClient.Post<string, EmotionDetailDto>(SystemConstant.EmotionDetailRoute, day);
        }

        public ApiResponse<string> GetToday()
        {
            return _stockHttpClient.Post<string>(SystemConstant.TodayRoute);
        }
    }
}
