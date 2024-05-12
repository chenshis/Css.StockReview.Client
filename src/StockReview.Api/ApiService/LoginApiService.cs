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

        public ApiResponse<bool?> ForgotPassword(ForgotPasswordRequestDto request)
        {
            request.Password = request.Password.GetMd5();
            request.ConfirmPassword = request.ConfirmPassword.GetMd5();
            var apiResponse = _stockHttpClient.Post<ForgotPasswordRequestDto, bool?>(SystemConstant.ForgotPasswordRoute, request);
            return apiResponse;
        }


        public ApiResponse<bool?> UpdatePassword(UpdatePasswordRequestDto request)
        {
            request.Password = request.Password.GetMd5();
            request.NewPassword = request.NewPassword.GetMd5();
            request.ConfirmPassword = request.ConfirmPassword.GetMd5();
            var apiResponse = _stockHttpClient.Post<UpdatePasswordRequestDto, bool?>(SystemConstant.UpdatePasswordRoute, request);
            return apiResponse;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ApiResponse<List<UserDto>> GetUsers(string keyword)
        {
            return _stockHttpClient.Get<List<UserDto>>(string.Format(SystemConstant.UsersRouteQuery, keyword));
        }

        public ApiResponse<bool?> UpdateUserRole(UpdateUserRoleRequestDto request)
        {
            return _stockHttpClient.Post<UpdateUserRoleRequestDto, bool?>(SystemConstant.UpdateUserRoleRoute, request);
        }

        public ApiResponse<bool?> AddUser(UserRequestDto request)
        {
            return _stockHttpClient.Post<UserRequestDto, bool?>(SystemConstant.AddUserRoute, request);
        }

        public ApiResponse<bool?> DeleteUser(string userName)
        {
            return _stockHttpClient.Post<string, bool?>(SystemConstant.UpdateUserRoleRoute, userName);
        }
    }
}
