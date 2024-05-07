using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockReview.Api.Dtos;

namespace StockReview.Server.Controllers
{
    /// <summary>
    /// 账户管理
    /// </summary>
    public class AccountController : StockReviewControllerBase
    {

        [HttpPost]
        [Route("v1/stock-review/account/login")]
        public async Task<string> Login([FromBody] AccountRequestDto accountRequest)
        {
            var client = new HttpClient();
            PasswordTokenRequest tokenRequest = new PasswordTokenRequest();
            tokenRequest.Address = "http://localhost:5245/connect/token";
            tokenRequest.GrantType = GrantType.ResourceOwnerPassword;
            tokenRequest.ClientId = "Zhaoxi.AspNetCore31.AuthDemo";
            tokenRequest.ClientSecret = "eleven123456";
            tokenRequest.Scope = "TestApi";
            tokenRequest.Parameters = new Dictionary<string, string>()
            {
                ["eMail"] = "57265177@qq.com",
                ["role"] = "Admin",
                [IdentityModel.JwtClaimTypes.NickName] = "Eleven"
            };
            tokenRequest.UserName = "Eleven";
            tokenRequest.Password = "123456";


            var tokenResponse = await client.RequestPasswordTokenAsync(tokenRequest);

            var token = tokenResponse.AccessToken;
            var tokenType = tokenResponse.TokenType;
            return "token";
        }
    }
}
