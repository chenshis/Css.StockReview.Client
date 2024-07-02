using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.IApiService
{
    /// <summary>
    /// 发送端
    /// </summary>
    public interface ISendMessageServerApiService
    {
        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="verificationPhone"></param>
        /// <returns></returns>
        bool GetVerificationCode(string verificationPhone);

        /// <summary>
        /// 发送注册短信
        /// </summary>
        /// <param name="phone"></param>
        void SendRegisterMessage(string phone);

        /// <summary>
        /// 发送找回密码消息
        /// </summary>
        /// <param name="phone"></param>
        void SendForgotMessage(string phone);
    }
}
