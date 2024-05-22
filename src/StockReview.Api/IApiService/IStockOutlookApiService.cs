using StockReview.Api.Dtos;
using StockReview.Infrastructure.Config;

namespace StockReview.Api.IApiService
{
    public interface IStockOutlookApiService
    {
        ApiResponse<BulletinBoardDto> GetBulletinBoard(string day);

        ApiResponse<EmotionDetailDto> GetEmotionDetail(string day);

        ApiResponse<string> GetToday();
    }
}
