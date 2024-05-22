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
using System.Drawing;
using System.Text.RegularExpressions;

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
            if (string.IsNullOrWhiteSpace(_memoryCache.Get<string>(SystemConstant.StockSelectedDayKey)))
            {
                FilterDates();
            }
            return _memoryCache.Get<string>(SystemConstant.StockSelectedDayKey);
        }

        public void FilterDates()
        {
            try
            {
                // 选中天
                var client = _httpClientFactory.CreateClient(SystemConstant.SpecialLonghuVipUrl);
                var content = GetFormUrlEncodedContent(a: "UpdateState", c: "UserSelectStock");
                var httpResponseMessage = client.PostAsync(default(string), content).Result;
                var strResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var jobject = (JObject)JsonConvert.DeserializeObject(strResult);
                var day = jobject["Day"]?.ToString();
                // 节假日
                List<string> filterDays = new List<string>();
                client = _httpClientFactory.CreateClient(SystemConstant.HistoryLonghuVipUrl);
                content = GetFormUrlEncodedContent(a: "GetHoliday", c: "YiDongKanPan");
                httpResponseMessage = client.PostAsync(default(string), content).Result;
                strResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
                jobject = (JObject)JsonConvert.DeserializeObject(strResult);
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
                var board = _memoryCache.Get<BulletinBoardDto>(SystemConstant.BulletinBoardKey);
                if (board != null)
                {
                    return board;
                }
            }
            return GetHisBulletinBoard(day);
        }

        public BulletinBoardDto GetHisBulletinBoard(string day)
        {
            // 检测日期合法性
            var cacheSelectedDay = _memoryCache.Get<string>(SystemConstant.StockSelectedDayKey);
            if (cacheSelectedDay == null)
            {
                throw new Exception("当前日期未获取到，请稍后重试！");
            }
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

        public EmotionDetailDto GetEmotionDetail(string day)
        {
            GetDateByDay(day);
            var today = _memoryCache.Get<string>(SystemConstant.StockSelectedDayKey);
            if (today != null && today == day)
            {
                return _memoryCache.Get<EmotionDetailDto>(SystemConstant.EmotionDetailKey);
            }
            return GetHisEmotionDetail(day);
        }

        public EmotionDetailDto GetHisEmotionDetail(string day)
        {
            EmotionDetailDto emotionDetail = new EmotionDetailDto();
            var httpClient = _httpClientFactory.CreateClient();
            var date = GetDateByDay(day, "yyyyMMdd");
            string text = date.today;

            int page = 1;
            List<Info> infoList = new List<Info>();
            HttpResponseMessage webDataResponse;
            string webData;
            string webUrl;
            while (true)
            {
                webUrl = $"https://data.10jqka.com.cn/dataapi/limit_up/limit_up_pool?page={page}&limit=200&field=199112,10,9001,330323,330324,330325,9002,330329,133971,133970,1968584,3475914,9003,9004&filter=HS,GEM2STAR&order_field=330324&order_type=0&date=" + text;
                webDataResponse = httpClient.GetAsync(webUrl).Result;
                webData = webDataResponse.Content.ReadAsStringAsync().Result;
                if (webData.Length < 650)
                {
                    break;
                }
                var tempRoot = JsonConvert.DeserializeObject<Root>(webData);
                if (emotionDetail.root == null)
                {
                    emotionDetail.root = tempRoot;
                }
                // 信息集合
                var infos = tempRoot.data.info;
                if (infos != null && infos.Count > 0)
                {
                    infoList.AddRange(infos);
                }
                int count = tempRoot.data.page.count;
                if (page < count)
                {
                    page++;
                    continue;
                }

                break;
            }

            // 获取连扳
            webUrl = "https://data.10jqka.com.cn/dataapi/limit_up/continuous_limit_up?filter=HS,GEM2STAR&date=" + text;
            webDataResponse = httpClient.GetAsync(webUrl).Result;
            webData = webDataResponse.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrWhiteSpace(webData))
            {
                var lBanData = JsonConvert.DeserializeObject<EmotionDetailLBanDto.Root>(webData);
                var codes = lBanData.data.SelectMany(t => t.code_list).ToList();
                foreach (var item in codes)
                {
                    foreach (var info in infoList)
                    {
                        if (info.code == item.code)
                        {
                            info.LBanNum = item.continue_num.ToString();
                        }
                    }
                }
            }

            // 一般信息赋值
            emotionDetail.root.data.info = infoList;

            // 柱状图
            webUrl = "http://hqstats.10jqka.com.cn/?market=USHA_USZA&datatype=zhangfustats_detail&date=" + text + "&callback=zhangfustats";
            webDataResponse = httpClient.GetAsync(webUrl).Result;
            webData = webDataResponse.Content.ReadAsStringAsync().Result;
            if (webData.Length >= 60)
            {
                List<string> yValues = ZZStr(webData);
                List<string> xValues = SystemConstant.TongHuaXValue.Split(",").ToList();

                emotionDetail.histogram = new List<HistogramDto>();

                for (int i = 0; i < xValues.Count; i++)
                {
                    emotionDetail.histogram.Add(new HistogramDto
                    {
                        xvalue = xValues[i],
                        yvalue = yValues[i]
                    });
                }
            }
            return emotionDetail;
        }

        private List<string> ZZStr(string str)
        {
            string[] array = Regex.Replace(str, "[^\\d.\\d]", ",").Split(new string[1] { "," }, StringSplitOptions.None);
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            string[] array2 = array;
            foreach (string text in array2)
            {
                if (text != "")
                {
                    list.Add(text);
                }
            }
            list2.Add(list[0]);
            list2.Add((Convert.ToInt32(list[1]) + Convert.ToInt32(list[1])).ToString());
            list2.Add((Convert.ToInt32(list[3]) + Convert.ToInt32(list[4])).ToString());
            list2.Add((Convert.ToInt32(list[5]) + Convert.ToInt32(list[6])).ToString());
            list2.Add((Convert.ToInt32(list[7]) + Convert.ToInt32(list[8]) + Convert.ToInt32(list[9])).ToString());
            list2.Add(list[10]);
            list2.Add((Convert.ToInt32(list[11]) + Convert.ToInt32(list[12]) + Convert.ToInt32(list[13])).ToString());
            list2.Add((Convert.ToInt32(list[14]) + Convert.ToInt32(list[15])).ToString());
            list2.Add((Convert.ToInt32(list[16]) + Convert.ToInt32(list[17])).ToString());
            list2.Add((Convert.ToInt32(list[18]) + Convert.ToInt32(list[19])).ToString());
            list2.Add(list[20]);
            return list2;
        }

        public (string today, string yesterday) GetDateByDay(string day, string format = "yyyy-MM-dd")
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
            return (date.ToString(format), yesterdayDate.ToString(format));
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
