using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    /// <summary>
    /// 忘记密码请求数据传输对象
    /// </summary>
    public class ForgotPasswordRequestDto
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
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
    }
}
