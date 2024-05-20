using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NLog;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace StockReview.Api.ApiService
{
    /// <summary>
    /// Stock Out Look Implementation
    /// </summary>
    public class StockOutlookServerApiService : IStockOutlookServerApiService
    {
        private readonly ILogger<StockOutlookServerApiService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;

        public StockOutlookServerApiService(ILogger<StockOutlookServerApiService> logger,
                                            IHttpClientFactory httpClientFactory,
                                            IMemoryCache memoryCache)
        {
            this._logger = logger;
            this._httpClientFactory = httpClientFactory;
            this._memoryCache = memoryCache;
        }

        public string GetCurrentDay()
        {
            // 选中天
            var client = _httpClientFactory.CreateClient(SystemConstant.SpecialLonghuVipUrl);
            var content = GetFormUrlEncodedContent(a: "UpdateState", c: "UserSelectStock");
            var httpResponseMessage = client.PostAsync(default(string), content).Result;
            var strResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var jobject = (JObject)JsonConvert.DeserializeObject(strResult);
            var day = jobject["Day"]?.ToString();
            return day;
        }

        public void FilterDates()
        {
            try
            {
                var day = GetCurrentDay();
                // 节假日
                List<string> filterDays = new List<string>();
                var client = _httpClientFactory.CreateClient(SystemConstant.HistoryLonghuVipUrl);
                var content = GetFormUrlEncodedContent(a: "GetHoliday", c: "YiDongKanPan");
                var httpResponseMessage = client.PostAsync(default(string), content).Result;
                var strResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var jobject = (JObject)JsonConvert.DeserializeObject(strResult);
                var jarrayList = (JArray)jobject["List"];
                foreach (JToken item in jarrayList)
                {
                    filterDays.Add(item.Value<string>());
                }
                // 更新缓存
                _memoryCache.Set(SystemConstant.StockSelectedDayKey, day);
                _memoryCache.Set(SystemConstant.StockFilterDaysKey, filterDays);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"过滤日期函数有问题：{ex.Message}");
            }

        }

        public BulletinBoardDto GetBulletinBoard(string day)
        {
            GetDateByDay(day);
            var today = _memoryCache.Get<string>(SystemConstant.StockSelectedDayKey);
            if (today != null && today == day)
            {
                return _memoryCache.Get<BulletinBoardDto>(SystemConstant.BulletinBoardKey);
            }
            return GetHisBulletinBoard(day);
        }

        public BulletinBoardDto GetHisBulletinBoard(string day)
        {
            // 检测日期合法性
            var cacheSelectedDay = _memoryCache.Get<string>(SystemConstant.StockSelectedDayKey);
            if (cacheSelectedDay == null)
            {
                FilterDates();
            }
            cacheSelectedDay = _memoryCache.Get<string>(SystemConstant.StockSelectedDayKey);

            var date = GetDateByDay(day);
            var today = date.today;
            var yesterday = date.yesterday;
            var httpClientToday = _httpClientFactory.CreateClient(SystemConstant.TodayLonghuVipUrl);
            var httpClientHis = _httpClientFactory.CreateClient(SystemConstant.HistoryLonghuVipUrl);

            BulletinBoardDto bulletinBoard = new();
            try
            {
                FormUrlEncodedContent formUrlContent;
                HttpResponseMessage responseMessage;
                JObject jobject;
                JObject jobjectInfo;
                JArray jArrayInfo;
                // 当天
                if (date.today == cacheSelectedDay)
                {
                    #region 涨幅详情
                    formUrlContent = GetFormUrlEncodedContent("ZhangFuDetail");
                    responseMessage = httpClientToday.PostAsync(default(string), formUrlContent).Result;
                    var zhangFuDetailStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(zhangFuDetailStrResult);
                    jobjectInfo = (JObject)jobject["info"];
                    bulletinBoard.TodayLimitUp = jobjectInfo["SJZT"]?.ToString() ?? "--";
                    bulletinBoard.TodayLimitDown = jobjectInfo["SJDT"]?.ToString() ?? "--";
                    bulletinBoard.TodayRise = jobjectInfo["SZJS"]?.ToString() ?? "--";
                    bulletinBoard.TodayFall = jobjectInfo["XDJS"]?.ToString() ?? "--";
                    bulletinBoard.TodayCalorimeter = Math.Round(Convert.ToDouble(jobjectInfo["qscln"].ToString()) / 10000.0, 0).ToString() + "亿";
                    #endregion

                    #region 北向资金
                    formUrlContent = GetFormUrlEncodedContent("NorthboundFundsB");
                    responseMessage = httpClientToday.PostAsync(default(string), formUrlContent).Result;
                    var northboundFundsBStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(northboundFundsBStrResult);
                    jobjectInfo = (JObject)jobject["info"];
                    if (jobjectInfo["day"] != null && jobjectInfo["day"].ToString() != DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        bulletinBoard.NorthboundFunds = "休市";
                    }
                    else
                    {
                        JArray jarray = (JArray)jobjectInfo["trend"];
                        if (jarray.Count == 0)
                        {
                            bulletinBoard.NorthboundFunds = "休市";
                        }
                        else
                        {
                            JArray jarray2 = (JArray)jarray[jarray.Count - 1];
                            string text2 = jarray2[jarray2.Count - 1].ToString();
                            double num = Convert.ToDouble(text2);
                            double num2 = Math.Round(num / 100000000.0, 2);
                            bulletinBoard.NorthboundFunds = num2.ToString() + "亿";
                        }
                    }
                    #endregion

                    #region 涨停板
                    formUrlContent = GetFormUrlEncodedContent("ZhangTingExpression");
                    responseMessage = httpClientToday.PostAsync(default(string), formUrlContent).Result;
                    var zhangTingExpStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(zhangTingExpStrResult);
                    jArrayInfo = (JArray)jobject["info"];
                    bulletinBoard.SecondBoardPercent = Math.Floor(Convert.ToDouble(jArrayInfo[4].ToString())).ToString() + "%";
                    bulletinBoard.ThirdBoardPercent = Math.Round(Convert.ToDouble(jArrayInfo[5].ToString())).ToString() + "%";
                    bulletinBoard.HighBoardPercent = Math.Round(Convert.ToDouble(jArrayInfo[6].ToString())).ToString() + "%";
                    bulletinBoard.TodayFryingRate = Math.Round(Convert.ToDouble(jArrayInfo[7].ToString())).ToString() + "%";
                    #endregion

                    #region 情绪
                    formUrlContent = GetFormUrlEncodedContent("DiskReview");
                    responseMessage = httpClientToday.PostAsync(default(string), formUrlContent).Result;
                    var diskReviewStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(diskReviewStrResult);
                    jobjectInfo = (JObject)jobject["info"];
                    bulletinBoard.EmotionPercent = jobjectInfo["strong"].ToString();
                    #endregion

                    #region 量能预测
                    formUrlContent = GetFormUrlEncodedContent("MarketCapacity");
                    responseMessage = httpClientToday.PostAsync(default(string), formUrlContent).Result;
                    var marketCapacityStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(marketCapacityStrResult);
                    jobjectInfo = (JObject)jobject["info"];
                    bulletinBoard.CityPower = jobjectInfo["yclnstr"].ToString();
                    #endregion
                }
                else
                {
                    #region 涨幅详情
                    formUrlContent = GetFormUrlEncodedContent(a: "HisZhangFuDetail", c: "HisHomeDingPan", Day: today);
                    responseMessage = httpClientHis.PostAsync(default(string), formUrlContent).Result;
                    var todayZhangFuDetailStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(todayZhangFuDetailStrResult);
                    jobjectInfo = (JObject)jobject["info"];
                    bulletinBoard.TodayLimitUp = jobjectInfo["SJZT"]?.ToString() ?? "--";
                    bulletinBoard.TodayLimitDown = jobjectInfo["SJDT"]?.ToString() ?? "--";
                    bulletinBoard.TodayRise = jobjectInfo["SZJS"]?.ToString() ?? "--";
                    bulletinBoard.TodayFall = jobjectInfo["XDJS"]?.ToString() ?? "--";
                    bulletinBoard.TodayCalorimeter = Math.Round(Convert.ToDouble(jobjectInfo["qscln"].ToString()) / 10000.0, 0).ToString() + "亿";
                    #endregion

                    #region 北向资金
                    formUrlContent = GetFormUrlEncodedContent("NorthboundFundsB", c: "HisHomeDingPan", Day: today);
                    responseMessage = httpClientHis.PostAsync(default(string), formUrlContent).Result;
                    var northboundFundsBStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(northboundFundsBStrResult);
                    jobjectInfo = (JObject)jobject["info"];
                    if (jobjectInfo["day"] != null && jobjectInfo["day"].ToString() != DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        bulletinBoard.NorthboundFunds = "休市";
                    }
                    else
                    {
                        JArray jarray = (JArray)jobjectInfo["trend"];
                        if (jarray.Count == 0)
                        {
                            bulletinBoard.NorthboundFunds = "休市";
                        }
                        else
                        {
                            JArray jarray2 = (JArray)jarray[jarray.Count - 1];
                            string text2 = jarray2[jarray2.Count - 1].ToString();
                            double num = Convert.ToDouble(text2);
                            double num2 = Math.Round(num / 100000000.0, 2);
                            bulletinBoard.NorthboundFunds = num2.ToString() + "亿";
                        }
                    }
                    #endregion

                    #region 涨停板
                    formUrlContent = GetFormUrlEncodedContent(a: "ZhangTingExpression", c: "HisHomeDingPan", Day: today);
                    responseMessage = httpClientHis.PostAsync(default(string), formUrlContent).Result;
                    var zhangTingExpStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(zhangTingExpStrResult);
                    jArrayInfo = (JArray)jobject["info"];
                    bulletinBoard.SecondBoardPercent = Math.Floor(Convert.ToDouble(jArrayInfo[4].ToString())).ToString() + "%";
                    bulletinBoard.ThirdBoardPercent = Math.Round(Convert.ToDouble(jArrayInfo[5].ToString())).ToString() + "%";
                    bulletinBoard.HighBoardPercent = Math.Round(Convert.ToDouble(jArrayInfo[6].ToString())).ToString() + "%";
                    bulletinBoard.TodayFryingRate = Math.Round(Convert.ToDouble(jArrayInfo[7].ToString())).ToString() + "%";
                    #endregion

                    #region 情绪
                    formUrlContent = GetFormUrlEncodedContent(a: "DiskReview", c: "HisHomeDingPan", Day: today);
                    responseMessage = httpClientHis.PostAsync(default(string), formUrlContent).Result;
                    var diskReviewStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(diskReviewStrResult);
                    jobjectInfo = (JObject)jobject["info"];
                    bulletinBoard.EmotionPercent = jobjectInfo["strong"].ToString();
                    #endregion

                    #region 量能预测
                    formUrlContent = GetFormUrlEncodedContent("MarketCapacity", "HisHomeDingPan", Date: today);
                    responseMessage = httpClientHis.PostAsync(default(string), formUrlContent).Result;
                    var marketCapacityStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                    jobject = (JObject)JsonConvert.DeserializeObject(marketCapacityStrResult);
                    jobjectInfo = (JObject)jobject["info"];
                    bulletinBoard.CityPower = jobjectInfo["yclnstr"].ToString();
                    #endregion
                }


                #region 昨天
                formUrlContent = GetFormUrlEncodedContent(a: "HisZhangFuDetail", c: "HisHomeDingPan", Day: yesterday);
                responseMessage = httpClientHis.PostAsync(default(string), formUrlContent).Result;
                var hisZhangFuDetailStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                jobject = (JObject)JsonConvert.DeserializeObject(hisZhangFuDetailStrResult);
                jobjectInfo = (JObject)jobject["info"];
                bulletinBoard.YesterdayLimitUp = jobjectInfo["SJZT"]?.ToString() ?? "--";
                bulletinBoard.YesterdayLimitDown = jobjectInfo["SJDT"]?.ToString() ?? "--";
                bulletinBoard.YesterdayRise = jobjectInfo["SZJS"]?.ToString() ?? "--";
                bulletinBoard.YesterdayFall = jobjectInfo["XDJS"]?.ToString() ?? "--";
                bulletinBoard.YesterdayCalorimeter = Math.Round(Convert.ToDouble(jobjectInfo["qscln"].ToString()) / 10000.0, 0).ToString() + "亿";

                formUrlContent = GetFormUrlEncodedContent(a: "ZhangTingExpression", c: "HisHomeDingPan", apiv: SystemConstant.apivW36, verSion: SystemConstant.VerSion51404, Day: yesterday);
                responseMessage = httpClientHis.PostAsync(default(string), formUrlContent).Result;
                var hisZhangTingExpStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                jobject = (JObject)JsonConvert.DeserializeObject(hisZhangTingExpStrResult);
                jArrayInfo = (JArray)jobject["info"];
                bulletinBoard.YesterdayFryingRate = Math.Round(Convert.ToDouble(jArrayInfo[7].ToString())).ToString() + "%";
                #endregion

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return bulletinBoard;
        }



        public (string today, string yesterday) GetDateByDay(string day)
        {
            if (!DateTime.TryParse(day, out var date))
            {
                throw new Exception("日期不合法！");
            }

            if (date > DateTime.Now)
            {
                throw new Exception("日期不合法！");
            }

            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                throw new Exception("日期不合法！");
            }

            var filterDays = _memoryCache.Get<List<string>>(SystemConstant.StockFilterDaysKey);
            if (filterDays == null)
            {
                throw new Exception("缺少节假日过滤！");
            }
            var filterDay = filterDays.Where(t => t == date.ToString("yyyy-MM-dd")).FirstOrDefault();
            if (filterDay != null)
            {
                throw new Exception("日期不合法！");
            }

            // 获取昨天
            DateTime yesterdayDate = date.AddDays(-1);
            while (true)
            {
                if (yesterdayDate.DayOfWeek == DayOfWeek.Sunday || yesterdayDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    yesterdayDate = yesterdayDate.AddDays(-1);
                    continue;
                }
                var filterYesterDay = filterDays.Where(t => t == yesterdayDate.ToString("yyyy-MM-dd")).FirstOrDefault();
                if (filterYesterDay != null)
                {
                    yesterdayDate = yesterdayDate.AddDays(-1);
                    continue;
                }
                break;
            }
            return (date.ToString("yyyy-MM-dd"), yesterdayDate.ToString("yyyy-MM-dd"));
        }


        private FormUrlEncodedContent GetFormUrlEncodedContent(string a,
                                                               string c = SystemConstant.HomeDingPan,
                                                               string apiv = SystemConstant.apivW31,
                                                               string phoneOSNew = SystemConstant.PhoneOSNew,
                                                               string deviceID = SystemConstant.DeviceID,
                                                               string verSion = SystemConstant.VerSion57012,
                                                               string Day = null,
                                                               string Date = null)
        {
            Dictionary<string, string> parameters = new()
            {
                [nameof(a)] = a,
                [nameof(c)] = c,
                [nameof(apiv)] = apiv,
                [nameof(phoneOSNew)] = phoneOSNew,
                [nameof(deviceID)] = deviceID,
                [nameof(verSion)] = verSion
            };

            if (!string.IsNullOrWhiteSpace(Day))
            {
                parameters[nameof(Day)] = Day;
            }
            if (!string.IsNullOrWhiteSpace(Date))
            {
                parameters[nameof(Date)] = Date;
            }

            var content = new FormUrlEncodedContent(parameters);
            return content;
        }

    }
}
