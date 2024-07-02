using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.ApiService
{
    /// <summary>
    /// Impl
    /// </summary>
    public class SendMessageServerApiService : ISendMessageServerApiService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<SendMessageOptions> _options;

        public SendMessageServerApiService(IMemoryCache memoryCache, IOptions<SendMessageOptions> options)
        {
            this._memoryCache = memoryCache;
            this._options = options;
        }
        public bool GetVerificationCode(string verificationPhone)
        {
            if (string.IsNullOrWhiteSpace(verificationPhone))
            {
                throw new Exception("验证码数据不存在！");
            }
            if (verificationPhone.Length != 17)
            {
                throw new Exception("验证码手机号格式不正确！");
            }
            var code = verificationPhone.Substring(0, 6);
            var phone = verificationPhone.Substring(6);
            var sendMessage = _memoryCache.Get<SendMessageCache>(string.Format(nameof(SendMessageCache), phone));
            if (sendMessage == null)
            {
                throw new Exception("验证码信息不存在！");
            }
            if (sendMessage.Code == code)
            {
                return true;
            }
            return false;
        }

        public void SendForgotMessage(string phone)
        {
            throw new NotImplementedException();
        }

        public void SendRegisterMessage(string phone)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 缓存发送信息
        /// </summary>
        private class SendMessageCache
        {
            /// <summary>
            /// 限制数量
            /// </summary>
            public int Limit { get; set; }

            /// <summary>
            /// 验证码
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// 手机号码
            /// </summary>
            public string Phone { get; set; }
        }
    }
}
