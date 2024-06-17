using StockReview.Api.Dtos;
using StockReview.Infrastructure.Config;
using System.Collections.Generic;

namespace StockReview.Api.IApiService
{
    public interface IStockOutlookApiService
    {
        ApiResponse<BulletinBoardDto> GetBulletinBoard(string day);

        ApiResponse<EmotionDetailDto> GetEmotionDetail(string day);

        ApiResponse<string> GetToday();

        ApiResponse<StockDto> GetStock(StockRequestDto request);

        ApiResponse<List<ConnectingBoardDto>> GetConnectingBoard(string day);
    }
}
