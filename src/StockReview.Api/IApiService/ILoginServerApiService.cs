using StockReview.Api.Dtos;
using StockReview.Domain.Entities;
using System.Collections.Generic;

namespace StockReview.Api.IApiService
{
    /// <summary>
    /// 服务端登录接口
    /// </summary>
    public interface ILoginServerApiService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="accountRequest">账户请求</param>
        /// <returns></returns>
        UserEntity Login(AccountRequestDto accountRequest);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        bool Register(RegisterRequestDto registerRequest);

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool ForgotPassword(ForgotPasswordRequestDto request);

        /// <summary>
        /// 根据角色过滤菜单
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        List<MenuDto> GetMenus(string role);
    }
}
