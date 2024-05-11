using StockReview.Api.Dtos;
using StockReview.Infrastructure.Config;
using System.Collections.Generic;

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

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiResponse<bool?> ForgotPassword(ForgotPasswordRequestDto request);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiResponse<bool?> UpdatePassword(UpdatePasswordRequestDto request);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        ApiResponse<List<UserDto>> GetUsers(string keyword);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiResponse<bool?> UpdateUserRole(UpdateUserRoleRequestDto request);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiResponse<bool?> AddUser(UserRequestDto request);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        ApiResponse<bool?> DeleteUser(string userName);
    }
}
