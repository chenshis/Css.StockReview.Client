using System;
using System.Collections.Generic;
using System.Text;

namespace StockReview.Api.Dtos
{
    /// <summary>
    /// 账户请求数据传输对象
    /// </summary>
    public class AccountRequestDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
