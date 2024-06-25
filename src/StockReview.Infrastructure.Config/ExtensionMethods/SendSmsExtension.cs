using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tea;

namespace StockReview.Infrastructure.Config.ExtensionMethods
{
    /// <summary>
    /// 发送消息扩展类
    /// </summary>
    public static class SendSmsExtension
    {

        private static AlibabaCloud.SDK.Dysmsapi20170525.Client _client = null;

        /// <summary>
        /// 发短信
        /// </summary>
        /// <param name="options"></param>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsResponse SendSms(this SendMessageOptions options, string phone, string code, bool isReg = true)
        {
            AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest sendSmsRequest;
            var client = CreateClient(options.AccessKeyId, options.AccessKeySecret, options.Endpoint);
            if (isReg)
            {
                sendSmsRequest = new()
                {
                    PhoneNumbers = phone,
                    SignName = options.RegSignName,
                    TemplateCode = options.RegTemplateCode,
                    TemplateParam = string.Format(options.TemplateParam, code)
                };
            }
            else
            {
                sendSmsRequest = new()
                {
                    PhoneNumbers = phone,
                    SignName = options.ForgotSignName,
                    TemplateCode = options.ForgotTemplateCode,
                    TemplateParam = string.Format(options.TemplateParam, code)
                };
            }
            try
            {
                return client.SendSms(sendSmsRequest);
            }
            catch (TeaException error)
            {
                Console.WriteLine(error.Message);
                // 诊断地址
                Console.WriteLine(error.Data["Recommend"]);
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
                return null;
            }
            catch (Exception _error)
            {
                TeaException error = new TeaException(new Dictionary<string, object>
                {
                    { "message", _error.Message }
                });
                // 此处仅做打印展示，请谨慎对待异常处理，在工程项目中切勿直接忽略异常。
                // 错误 message
                Console.WriteLine(error.Message);
                // 诊断地址
                Console.WriteLine(error.Data["Recommend"]);
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
                return null;
            }
        }


        private static AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient(string accessKeyId, string accessKeySecret, string endPoint)
        {
            if (_client == null)
            {
                AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
                {
                    AccessKeyId = accessKeyId,
                    AccessKeySecret = accessKeySecret
                };
                config.Endpoint = endPoint;
                _client = new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
            }
            return _client;
        }
    }
}
