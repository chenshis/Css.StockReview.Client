using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using StockReview.Infrastructure.Config.ExtensionMethods;
using StockReview.Infrastructure.Config.HttpClients;
using System.Collections.Generic;

namespace StockReview.Api.ApiService
{
    public class LoginApiService : ILoginApiService
    {
        private readonly StockHttpClient _stockHttpClient;

        public LoginApiService(StockHttpClient stockHttpClient)
        {
            this._stockHttpClient = stockHttpClient;
        }
        public ApiResponse<List<MenuDto>> GetMenus()
            => _stockHttpClient.Post<List<MenuDto>>(SystemConstant.MenuRoute);

        public ApiResponse<string> Login(string username, string password)
        {
            var passwordMd5 = password.GetMd5();
            var apiResponse = _stockHttpClient.Post<AccountRequestDto, string>(SystemConstant.LoginRoute, new AccountRequestDto
            {
                UserName = username,
                Password = passwordMd5
            });
            if (apiResponse.Code == 0 && apiResponse.Data != null)
            {
                _stockHttpClient.SetToken(apiResponse.Data.ToString());
            }
            return apiResponse;
        }

        public ApiResponse<bool?> Register(RegisterRequestDto registerRequest)
        {
            registerRequest.Password = registerRequest.Password.GetMd5();
            var apiResponse = _stockHttpClient.Post<RegisterRequestDto, bool?>(SystemConstant.RegisterRoute, registerRequest);
            return apiResponse;
        }
    }
}
