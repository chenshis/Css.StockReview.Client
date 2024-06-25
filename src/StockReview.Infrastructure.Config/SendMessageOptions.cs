
namespace StockReview.Infrastructure.Config
{
    /// <summary>
    /// 发送消息选项
    /// </summary>
    public class SendMessageOptions
    {
        /// <summary>
        /// secret
        /// </summary>
        public string AccessKeySecret { get; set; }
        /// <summary>
        /// key
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// 注册签名
        /// </summary>
        public string RegSignName { get; set; }

        /// <summary>
        /// 注册模板编号
        /// </summary>
        public string RegTemplateCode { get; set; }

        /// <summary>
        /// 忘记签名
        /// </summary>
        public string ForgotSignName { get; set; }

        /// <summary>
        /// 忘记模板编号
        /// </summary>
        public string ForgotTemplateCode { get; set; }

        /// <summary>
        /// 模板参数
        /// </summary>

        public string TemplateParam { get; set; }

        /// <summary>
        /// 每天限流次数
        /// </summary>
        public string Limit { get; set; }
    }
}
