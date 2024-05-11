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
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool UpdatePassword(UpdatePasswordRequestDto request);

        /// <summary>
        /// 根据角色过滤菜单
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        List<MenuDto> GetMenus(string role);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        List<UserDto> GetUsers(string keyword);


        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool UpdateUserRole(UpdateUserRoleRequestDto request);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool AddUser(UserRequestDto request);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool DeleteUser(string userName);
    }
}
