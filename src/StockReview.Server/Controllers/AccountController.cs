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
        public string Login([FromBody] AccountRequestDto accountRequest)
        {
            return "token";
        }
    }
}
