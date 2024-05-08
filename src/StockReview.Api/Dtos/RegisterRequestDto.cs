
namespace StockReview.Api.Dtos
{
    /// <summary>
    /// 注册请求数据传输对象
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
    }
}
