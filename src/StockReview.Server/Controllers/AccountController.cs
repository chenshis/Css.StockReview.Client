using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockReview.Api.Dtos;
using StockReview.Domain;
using StockReview.Infrastructure.Config;
using System.Net.Http.Headers;

namespace StockReview.Server.Controllers
{
    /// <summary>
    /// 账户管理
    /// </summary>
    public class AccountController : StockReviewControllerBase
    {
        private readonly StockReviewDbContext _dbContext;

        public AccountController(StockReviewDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("v1/stock-review/account/login")]
        public async Task<string> Login([FromBody] AccountRequestDto accountRequest)
        {
            var client = new HttpClient();
            PasswordTokenRequest tokenRequest = new PasswordTokenRequest();
            tokenRequest.Address = "http://localhost:5245/connect/token";
            tokenRequest.GrantType = GrantType.ResourceOwnerPassword;
            tokenRequest.ClientId = SystemConstant.IdentityServerClient;
            tokenRequest.ClientSecret = SystemConstant.IdentityServerSecret;
            tokenRequest.Scope = SystemConstant.IdentityServerScope;
            tokenRequest.Parameters = new Dictionary<string, string>()
            {
                ["role"] = "8",
                [IdentityModel.JwtClaimTypes.NickName] = "管理员"
            };
            tokenRequest.UserName = "admin";
            tokenRequest.Password = "admin123456";


            var tokenResponse = await client.RequestPasswordTokenAsync(tokenRequest);

            var token = tokenResponse.AccessToken;
            var tokenType = tokenResponse.TokenType;


            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{tokenType} {token}");
            var response = client.PostAsync("http://localhost:5245/v1/stock-review/account/index", null).Result;


            return "token";
        }

        [Authorize]
        [HttpPost]
        [Route("v1/stock-review/account/index")]
        public string Index()
        {
            return "";
        }
    }
}
