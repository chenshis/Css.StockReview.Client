using StockReview.Api.Dtos;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace StockReview.Api.IApiService
{
    /// <summary>
    /// 菜单接口
    /// </summary>
    public interface ILoginApiService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        ApiResponse<string> Login(string username, string password);

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        ApiResponse<List<MenuDto>> GetMenus();

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        ApiResponse<bool?> Register(RegisterRequestDto registerRequest);
    }
}
