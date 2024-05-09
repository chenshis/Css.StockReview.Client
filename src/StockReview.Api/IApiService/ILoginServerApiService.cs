using StockReview.Api.Dtos;
using StockReview.Domain.Entities;

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
    }
}
