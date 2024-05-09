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

        public ApiResponse Get(string route)
        {
            try
            {
                HttpResponseMessage responseMessage = _httpClient.GetAsync(route).Result;
                var responseResult = GetResponseCodeResult(responseMessage);
                // 未授权 但是有token 则刷新token
                if (responseResult.Code == (int)System.Net.HttpStatusCode.Unauthorized)
                {
                    responseResult = RefreshToken();
                    if (responseResult.Code != 0)
                    {
                        return responseResult;
                    }
                    responseMessage = _httpClient.GetAsync(route).Result;
                    responseResult = GetResponseCodeResult(responseMessage);
                    if (responseResult.Code != 0)
                    {
                        responseResult.Code = 1;
                    }
                }
                return responseResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Get请求异常：{ex.Message}");
                return new ApiResponse(1, ex.Message, ex);
            }
        }


        public ApiResponse Post(string route)
        {
            return Post(route, 0);
        }

        public ApiResponse Post<TRequest>(string route, TRequest request)
        {
            StringContent stringContent = null;
            if (request != null)
            {
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            }
            try
            {
                HttpResponseMessage responseMessage = _httpClient.PostAsync(route, stringContent).Result;
                var responseResult = GetResponseCodeResult(responseMessage);
                // 未授权 但是有token 则刷新token
                if (responseResult.Code == (int)System.Net.HttpStatusCode.Unauthorized)
                {
                    responseResult = RefreshToken();
                    if (responseResult.Code != 0)
                    {
                        return responseResult;
                    }
                    responseMessage = _httpClient.PostAsync(route, stringContent).Result;
                    responseResult = GetResponseCodeResult(responseMessage);
                    if (responseResult.Code != 0)
                    {
                        responseResult.Code = 1;
                    }
                }
                return responseResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Post请求异常：{ex.Message}");
                return new ApiResponse(1, ex.Message, ex);
            }
        }


        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        private ApiResponse RefreshToken()
        {
            ApiResponse apiResponse = null;
            var token = _httpClient.DefaultRequestHeaders.Authorization?.Parameter;
            if (string.IsNullOrWhiteSpace(token))
            {
                apiResponse = new ApiResponse(1, SystemConstant.Unauthorized, System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(token), Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage responseMessage = _httpClient.PostAsync(SystemConstant.RefreshTokenRoute, stringContent).Result;
                    apiResponse = GetResponseCodeResult(responseMessage);
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
                    apiResponse = new ApiResponse(1, ex.Message, ex);
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
        private ApiResponse GetResponseCodeResult(HttpResponseMessage responseMessage)
        {
            ApiResponse apiResponse = null;
            if (responseMessage.IsSuccessStatusCode)
            {
                try
                {
                    var stringResult = responseMessage.Content.ReadAsStringAsync().Result;
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(stringResult);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"接口请求序列化异常：{ex.Message}");
                    apiResponse = new ApiResponse(1, ex.Message, ex);
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
                        apiResponse = new ApiResponse(1, SystemConstant.Unauthorized, responseMessage.StatusCode);
                    }
                    else
                    {
                        // 赋值401状态 用于上层处理
                        apiResponse = new ApiResponse((int)responseMessage.StatusCode, SystemConstant.Unauthorized, responseMessage.StatusCode);
                    }
                }
                else
                {
                    apiResponse = new ApiResponse(1, responseMessage.ReasonPhrase, responseMessage.StatusCode);
                }
            }
            return apiResponse;
        }
    }
}
