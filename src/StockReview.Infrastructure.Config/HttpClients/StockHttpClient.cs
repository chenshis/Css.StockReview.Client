using IdentityModel.Client;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace StockReview.Infrastructure.Config.HttpClients
{
    public class StockHttpClient
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public StockHttpClient(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this._logger = loggerFactory.CreateLogger(this.GetType().FullName);
            this._httpClient = httpClientFactory.CreateClient(nameof(StockHttpClient));
            this._httpClient.BaseAddress = new Uri(configuration[SystemConstant.StockServerUrl]);
        }

        public StockHttpClient SetToken(string token)
        {
            this._httpClient.SetBearerToken(token);
            return this;
        }

        public ApiResponse<TData> Get<TData>(string route)
        {
            try
            {
                HttpResponseMessage responseMessage = _httpClient.GetAsync(route).Result;
                var responseResult = GetResponseCodeResult<TData>(responseMessage);
                // 未授权 但是有token 则刷新token
                if (responseResult.Code == (int)System.Net.HttpStatusCode.Unauthorized)
                {
                    var refreshResponse = RefreshToken();
                    if (refreshResponse.Code != 0)
                    {
                        _logger.LogError($"Get请求异常：{refreshResponse.Msg}");
                        return responseResult;
                    }
                    responseMessage = _httpClient.GetAsync(route).Result;
                    responseResult = GetResponseCodeResult<TData>(responseMessage);
                    if (responseResult.Code != 0)
                    {
                        _logger.LogError($"Get请求异常：{refreshResponse.Msg}");
                        responseResult.Code = 1;
                    }
                }
                return responseResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Get请求异常：{ex.Message}");
                var apiResponse = new ApiResponse<TData>();
                apiResponse.Code = 1;
                apiResponse.Msg = ex.Message;
                apiResponse.Data = default(TData);
                apiResponse.ServerTime = DateTime.Now.Ticks;
                return apiResponse;
            }
        }


        public ApiResponse<TData> Post<TData>(string route)
        {
            return Post<int, TData>(route, 0);
        }

        public ApiResponse<TData> Post<TRequest, TData>(string route, TRequest request)
        {
            StringContent stringContent = null;
            if (request != null)
            {
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            }
            try
            {
                HttpResponseMessage responseMessage = _httpClient.PostAsync(route, stringContent).Result;
                var responseResult = GetResponseCodeResult<TData>(responseMessage);
                // 未授权 但是有token 则刷新token
                if (responseResult.Code == (int)System.Net.HttpStatusCode.Unauthorized)
                {
                    var refreshResponse = RefreshToken();
                    if (refreshResponse.Code != 0)
                    {
                        _logger.LogError($"Post请求异常：{refreshResponse.Msg}");
                        return responseResult;
                    }
                    responseMessage = _httpClient.PostAsync(route, stringContent).Result;
                    responseResult = GetResponseCodeResult<TData>(responseMessage);
                    if (responseResult.Code != 0)
                    {
                        _logger.LogError($"Post请求异常：{responseResult.Msg}");
                        responseResult.Code = 1;
                    }
                }
                return responseResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Post请求异常：{ex.Message}");
                var apiResponse = new ApiResponse<TData>();
                apiResponse.Code = 1;
                apiResponse.Msg = ex.Message;
                apiResponse.Data = default(TData);
                apiResponse.ServerTime = DateTime.Now.Ticks;
                return apiResponse;
            }
        }


        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        private ApiResponse<string> RefreshToken()
        {
            ApiResponse<string> apiResponse = null;
            var token = _httpClient.DefaultRequestHeaders.Authorization?.Parameter;
            if (string.IsNullOrWhiteSpace(token))
            {
                apiResponse = new ApiResponse<string>();
                apiResponse.Code = 1;
                apiResponse.Msg = SystemConstant.Unauthorized;
                apiResponse.Data = default(string);
                apiResponse.ServerTime = DateTime.Now.Ticks;
            }
            else
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(token), Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage responseMessage = _httpClient.PostAsync(SystemConstant.RefreshTokenRoute, stringContent).Result;
                    apiResponse = GetResponseCodeResult<string>(responseMessage);
                    if (apiResponse.Code == 0)
                    {
                        SetToken(apiResponse.Data.ToString());
                    }
                    else
                    {
                        apiResponse.Code = 1;
                        SetToken("");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Post请求异常：{ex.Message}");
                    apiResponse = new ApiResponse<string>();
                    apiResponse.Code = 1;
                    apiResponse.Msg = ex.Message;
                    apiResponse.Data = default(string);
                    apiResponse.ServerTime = DateTime.Now.Ticks;
                }

            }
            return apiResponse;
        }


        /// <summary>
        /// 获取响应状态返回结果
        /// </summary>
        /// <param name="responseMessage">响应消息</param>
        /// <param name="route">路由</param>
        /// <returns></returns>
        private ApiResponse<TData> GetResponseCodeResult<TData>(HttpResponseMessage responseMessage)
        {
            ApiResponse<TData> apiResponse = null;
            if (responseMessage.IsSuccessStatusCode)
            {
                try
                {
                    var stringResult = responseMessage.Content.ReadAsStringAsync().Result;
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse<TData>>(stringResult);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"接口请求序列化异常：{ex.Message}");
                    apiResponse = new ApiResponse<TData>();
                    apiResponse.Code = 1;
                    apiResponse.Msg = ex.Message;
                    apiResponse.Data = default(TData);
                    apiResponse.ServerTime = DateTime.Now.Ticks;
                }
            }
            else
            {
                _logger.LogError($"接口请求错误,错误代码{responseMessage.StatusCode}，错误原因{responseMessage.ReasonPhrase}");
                // 401 访问拒绝
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var authorization = _httpClient.DefaultRequestHeaders.Authorization?.ToString();
                    if (string.IsNullOrWhiteSpace(authorization))
                    {
                        apiResponse = new ApiResponse<TData>();
                        apiResponse.Code = 1;
                        apiResponse.Msg = SystemConstant.Unauthorized;
                        apiResponse.Data = default(TData);
                        apiResponse.ServerTime = DateTime.Now.Ticks;
                    }
                    else
                    {
                        // 赋值401状态 用于上层处理
                        apiResponse = new ApiResponse<TData>();
                        apiResponse.Code = (int)responseMessage.StatusCode;
                        apiResponse.Msg = SystemConstant.Unauthorized;
                        apiResponse.Data = default(TData);
                        apiResponse.ServerTime = DateTime.Now.Ticks;
                    }
                }
                else
                {
                    apiResponse = new ApiResponse<TData>();
                    apiResponse.Code = 1;
                    apiResponse.Msg = responseMessage.ReasonPhrase;
                    apiResponse.Data = default(TData);
                    apiResponse.ServerTime = DateTime.Now.Ticks;
                }
            }
            return apiResponse;
        }
    }
}
