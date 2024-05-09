using System;

namespace StockReview.Infrastructure.Config
{
    /// <summary>
    /// Api响应
    /// </summary>
    public class ApiResponse : ApiResponseBase
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public virtual object Data { get; set; }

        public ApiResponse()
        {
            
        }

        public ApiResponse(int code, string msg, object data)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
            this.ServerTime = DateTime.Now.Ticks;
        }

        public ApiResponse(int code, string msg, object data, long serverTimeTicks)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
            this.ServerTime = serverTimeTicks;
        }
    }


    public class ApiResponseBase
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 服务端时间ticks
        /// </summary>
        public long ServerTime { get; set; }
    }
}
