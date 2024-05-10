using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using System.Security.Claims;

namespace StockReview.Server.Controllers
{
    /// <summary>
    /// 账户管理
    /// </summary>
    public class AccountController : StockReviewControllerBase
    {
        private readonly ILoginServerApiService _loginServerApiService;
        private readonly IJWTApiService _jWTApiService;

        public AccountController(ILoginServerApiService loginServerApiService, IJWTApiService jWTApiService)
        {
            this._loginServerApiService = loginServerApiService;
            this._jWTApiService = jWTApiService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="accountRequest">账户信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route(SystemConstant.LoginRoute)]
        public string Login([FromBody] AccountRequestDto accountRequest)
        {
            var user = _loginServerApiService.Login(accountRequest);
            return _jWTApiService.GetToken(user);
        }

        /// <summary>
        /// token刷新
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(SystemConstant.RefreshTokenRoute)]
        public string RefreshToken([FromBody] string token)
        {
            return _jWTApiService.RefreshToken(token);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="registerRequest">注册信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route(SystemConstant.RegisterRoute)]
        public bool Register([FromBody] RegisterRequestDto registerRequest)
        {
            return _loginServerApiService.Register(registerRequest);
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(SystemConstant.ForgotPasswordRoute)]
        public bool ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            return _loginServerApiService.ForgotPassword(request);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(SystemConstant.UpdatePasswordRoute)]
        public bool UpdatePassword([FromBody] UpdatePasswordRequestDto request)
        {
            return _loginServerApiService.UpdatePassword(request);
        }

        /// <summary>
        /// 菜单路由
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(SystemConstant.MenuRoute)]
        [Authorize]
        public List<MenuDto> PostMenus()
        {
            var role = HttpContext.User.Claims.Where(t => t.Type == ClaimTypes.Role).Select(t => t.Value).FirstOrDefault();
            return _loginServerApiService.GetMenus(role);
        }
    }
}
