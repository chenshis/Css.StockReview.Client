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
using System.Text.RegularExpressions;
using System.Collections;

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
                    bulletinBoard.TodayZTPBRate = Math.Round(Convert.ToDouble(jArrayInfo[7].ToString()), 3).ToString() + "%";
                    bulletinBoard.YesterdayZTJBX = Math.Round(Convert.ToDouble(jArrayInfo[8].ToString()),3).ToString() + "%";
                    bulletinBoard.YesterdayLBJBX = Math.Round(Convert.ToDouble(jArrayInfo[9].ToString()),3).ToString() + "%";
                    bulletinBoard.YesterdayPBJBX = Math.Round(Convert.ToDouble(jArrayInfo[10].ToString()),3).ToString() + "%";
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
                    bulletinBoard.TodayZTPBRate = Math.Round(Convert.ToDouble(jArrayInfo[7].ToString()), 3).ToString() + "%";
                    bulletinBoard.YesterdayZTJBX = Math.Round(Convert.ToDouble(jArrayInfo[8].ToString()), 3).ToString() + "%";
                    bulletinBoard.YesterdayLBJBX = Math.Round(Convert.ToDouble(jArrayInfo[9].ToString()), 3).ToString() + "%";
                    bulletinBoard.YesterdayPBJBX = Math.Round(Convert.ToDouble(jArrayInfo[10].ToString()), 3).ToString() + "%";
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

        public List<StockDataDto> GetStockDatas(string stockId)
        {
            string url = $"https://hq.stock.sohu.com/mkline/cn/{stockId.Substring(3)}/cn_{stockId}-10_2.html?_={GetTimespan()}";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(SystemConstant.StockUserAgent);
            var responseMessage = httpClient.GetAsync(url).Result;
            responseMessage.EnsureSuccessStatusCode();
            var content = responseMessage.Content.ReadAsStringAsync().Result.Replace("quote_d_dividend(", "").Replace("]]})", "]]}");
            JObject jObject = JsonConvert.DeserializeObject(content) as JObject;
            var jArray = jObject["dataDiv"] as JArray;
            List<StockDataDto> stocks = new();
            for (int i = 0; i < jArray.Count; i++)
            {
                JArray stockJarray = (JArray)jArray[i];
                string strDate = stockJarray[0].ToString();
                DateTime dateTime = Convert.ToDateTime(
                    new DateTime(int.Parse(strDate.Substring(0, 4)),
                                 int.Parse(strDate.Substring(4, 2).TrimStart(new char[] { '0' })),
                                 int.Parse(strDate.Substring(6, 2).TrimStart(new char[] { '0' }))));
                double openNum = double.Parse(stockJarray[1].ToString());
                double closeNum = double.Parse(stockJarray[2].ToString());
                double highPrice = double.Parse(stockJarray[3].ToString());
                double lowPrice = double.Parse(stockJarray[4].ToString());
                double volTurnover = Convert.ToDouble(stockJarray[5].ToString());
                double upDown = Convert.ToDouble(stockJarray[9].ToString().Replace("%", ""));
                stocks.Add(new StockDataDto
                {
                    Date = dateTime,
                    Open = openNum,
                    High = highPrice,
                    Low = lowPrice,
                    Close = closeNum,
                    Volume = volTurnover,
                    UpDown = upDown
                });
            }

            if (stocks.Count > 0)
            {
                var today = GetCurrentDay().Replace("-", "");
                var lastDay = jArray[0][0].ToString();
                if (today != lastDay)
                {
                    var stockData = GetTodayStockData(stockId);
                    if (stockData.Date.ToString("yyyyMMdd") != lastDay)
                    {
                        stocks.Insert(0, stockData);
                    }
                }
                GetAverageDays(stocks);
            }

            return stocks;
        }

        public StockDetailDataDto GetStockDetails(string stockId, string day)
        {
            StockDetailDataDto stockDetail = new();
            var today = GetCurrentDay();
            if (today == day)
            {
                var url = $"https://hq.stock.sohu.com/cn/{stockId.Substring(3)}/cn_{stockId}-4.html?openinwebview_finance=false&t={GetTimespan()}";
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(SystemConstant.StockUserAgent);
                var responseMessage = httpClient.GetAsync(url).Result;
                responseMessage.EnsureSuccessStatusCode();
                var content = GetUrlFormat(responseMessage.Content.ReadAsStringAsync().Result);
                // 数组
                var array = GetUrlFormat3(GetUrlFormat2(content, "'quote_m',\"", "\"]"), "<|||>");
                if (array.Length != 2 || !array[0].StartsWith("['init_") || !array[1].StartsWith("['end_"))
                {
                    content = content.Replace("time_data(", "").Replace(")", "");
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(content);
                    for (int i = 0; i < jarray.Count; i++)
                    {
                        JArray jarray2 = (JArray)jarray[i];
                        if (i == 0)
                        {
                            stockDetail.ClosePrice = double.Parse(jarray2[0].ToString());
                            stockDetail.HighPrice = double.Parse(jarray2[2].ToString());
                            stockDetail.LowPrice = double.Parse(jarray2[3].ToString());
                        }
                        else
                        {
                            stockDetail.Volumes.Add(double.Parse(jarray2[4].ToString()));
                            stockDetail.Times.Add(jarray2[0].ToString());
                            stockDetail.Latests.Add(double.Parse(jarray2[1].ToString()));
                            stockDetail.Avgs.Add(double.Parse(jarray2[2].ToString()));
                            stockDetail.Turnovers.Add(double.Parse(jarray2[3].ToString()));
                        }
                    }
                    for (int j = 0; j < stockDetail.Latests.Count; j++)
                    {
                        double d = stockDetail.Latests[j];
                        if (j == 0)
                        {
                            if (d > stockDetail.ClosePrice)
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[j], ColorEnum.Red));
                            }
                            else if (d < stockDetail.ClosePrice)
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[j], ColorEnum.Green));
                            }
                            else
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[j], ColorEnum.Gray));
                            }
                        }
                        else
                        {
                            double d2 = stockDetail.Latests[j - 1];
                            if (d > d2)
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[j], ColorEnum.Red));
                            }
                            else if (d < d2)
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[j], ColorEnum.Green));
                            }
                            else
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[j], ColorEnum.Gray));
                            }
                        }
                    }
                }
            }
            else
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(SystemConstant.UserAgent);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["a"] = "GetStockTrend";
                parameters["c"] = "StockL2History";
                parameters["StockID"] = stockId;
                parameters["Day"] = day;
                var formContent = new FormUrlEncodedContent(parameters);
                var httpResponseMessage = httpClient.PostAsync("https://apphis.longhuvip.com/w1/api/index.php", formContent).Result;
                httpResponseMessage.EnsureSuccessStatusCode();
                var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                JObject jobject = (JObject)JsonConvert.DeserializeObject(content);
                stockDetail.OpenPrice = double.Parse(jobject["begin_px"]?.ToString() ?? "0.0");
                stockDetail.HighPrice = double.Parse(jobject["hprice"]?.ToString() ?? "0.0");
                stockDetail.LowPrice = double.Parse(jobject["lprice"]?.ToString() ?? "0.0");
                stockDetail.ClosePrice = double.Parse(jobject["preclose_px"]?.ToString() ?? "0.0");

                var totalTurnover = jobject["total_turnover"];
                if (totalTurnover != null)
                {
                    stockDetail.TotalTurnover = long.Parse(totalTurnover.ToString());
                }
                else
                {
                    stockDetail.TotalTurnover = 0;
                }
                JArray jarray = (JArray)jobject["trend"];
                if (jarray != null)
                {
                    for (int i = 0; i < jarray.Count; i++)
                    {
                        JArray jarray2 = (JArray)jarray[i];
                        double latest = double.Parse(jarray2[1].ToString());
                        stockDetail.Times.Add(jarray2[0].ToString());
                        stockDetail.Latests.Add(latest);
                        stockDetail.Avgs.Add(double.Parse(jarray2[2].ToString()));
                        stockDetail.Turnovers.Add(double.Parse(jarray2[3].ToString()));
                        stockDetail.Volumes.Add(double.Parse(jarray2[4].ToString()));
                        if (i == 0)
                        {
                            if (latest > stockDetail.ClosePrice)
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[i], ColorEnum.Red));
                            }
                            else if (latest < stockDetail.ClosePrice)
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[i], ColorEnum.Green));
                            }
                            else
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[i], ColorEnum.Red));
                            }
                        }
                        else
                        {
                            double d = double.Parse(((JArray)jarray[i - 1])[1].ToString());
                            if (latest > d)
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[i], ColorEnum.Red));
                            }
                            else if (latest < d)
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[i], ColorEnum.Green));
                            }
                            else
                            {
                                stockDetail.Colors.Add(new StockDetailColorDto(stockDetail.Turnovers[i], ColorEnum.Gray));
                            }
                        }
                    }
                }
            }
            return stockDetail;
        }

        public StockDto GetStock(StockRequestDto request)
        {
            var stockMemory = _memoryCache.Get<StockDto>(string.Concat(request.StockId, request.Day));
            if (stockMemory != null)
            {
                return stockMemory;
            }
            StockDto stock = new();
            stock.StockId = request.StockId;
            stock.Date = request.Day;
            //日期过滤
            if (DateTime.TryParse(request.Day, out DateTime dateTime))
            {
                stock.StockDatas = GetStockDatas(request.StockId);
                stock.StockDatas = stock.StockDatas.Where(t => t.Date <= dateTime).OrderByDescending(t => t.Date).Take(100).ToList();
                stock.StockDetailData = GetStockDetails(request.StockId, request.Day);
                _memoryCache.Set(string.Concat(request.StockId, request.Day),
                                 stock,
                                 new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }
            return stock;
        }


        #region 私有方法

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimespan()
        {
            DateTime d = new DateTime(1970, 1, 1);
            return (DateTime.Now.AddHours(-8.0) - d).TotalMilliseconds.ToString("f0");
        }

        /// <summary>
        /// 获取当天
        /// </summary>
        /// <returns></returns>
        private StockDataDto GetTodayStockData(string stockId)
        {
            string url = "https://hqm.stock.sohu.com/getqjson?code=cn_" + stockId;
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(SystemConstant.StockUserAgent);
            var responseMessage = httpClient.GetAsync(url).Result;
            responseMessage.EnsureSuccessStatusCode();
            var content = responseMessage.Content.ReadAsStringAsync().Result;
            JObject jobject = (JObject)JsonConvert.DeserializeObject(content);
            JArray jarray = (JArray)jobject["cn_" + stockId];
            StockDataDto stockDataDto = new();
            stockDataDto.Open = Convert.ToDouble(jarray[14].ToString());
            stockDataDto.Close = Convert.ToDouble(jarray[2].ToString());
            stockDataDto.UpDown = Convert.ToDouble(jarray[3].ToString().Replace("%", ""));
            stockDataDto.Low = Convert.ToDouble(jarray[11].ToString());
            stockDataDto.High = Convert.ToDouble(jarray[10].ToString());
            stockDataDto.Volume = Convert.ToDouble(jarray[5].ToString());
            stockDataDto.Date = Convert.ToDateTime(jarray[17].ToString());

            return stockDataDto;
        }

        private void GetAverageDays(List<StockDataDto> stockDatas)
        {
            if (stockDatas == null || stockDatas.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < stockDatas.Count; i++)
            {
                var stockData = stockDatas[i];
                double m5 = this.GetAverageDaysDetail(stockDatas, i, 5);
                double m10 = this.GetAverageDaysDetail(stockDatas, i, 10);
                double m20 = this.GetAverageDaysDetail(stockDatas, i, 20);
                double m30 = this.GetAverageDaysDetail(stockDatas, i, 30);
                stockData.M5 = m5;
                stockData.M10 = m10;
                stockData.M20 = m20;
                stockData.M30 = m30;
            }
        }

        /// <summary>
        /// 获取平均天数详情
        /// </summary>
        /// <param name="stockDatas"></param>
        /// <param name="index"></param>
        /// <param name="mNum"></param>
        /// <returns></returns>
        private double GetAverageDaysDetail(List<StockDataDto> stockDatas, int index, int mNum)
        {
            double result;
            if (index + mNum > stockDatas.Count)
            {
                result = 0d;
            }
            else
            {
                double d = 0d;
                for (int i = index; i < index + mNum; i++)
                {
                    d += stockDatas[i].Close;
                }
                result = Math.Round(d / mNum, 2);
            }
            return result;
        }

        /// <summary>
        /// url格式处理
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetUrlFormat(string content)
        {
            string result;
            try
            {
                string pattern = string.Concat(new string[]
                {
                "(",
                Convert.ToChar(8).ToString(),
                "|",
                Convert.ToChar(9).ToString(),
                "|",
                Convert.ToChar(10).ToString(),
                "|",
                Convert.ToChar(13).ToString(),
                ")"
                });
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                content = regex.Replace(content, "");
                result = content;
            }
            catch
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// url格式处理2
        /// </summary>
        /// <param name="string_1"></param>
        /// <param name="string_2"></param>
        /// <param name="string_3"></param>
        /// <returns></returns>
        private string GetUrlFormat2(string string_1, string string_2, string string_3)
        {
            string text = "";
            ArrayList arrayList = new ArrayList();
            string result;
            if (string_2.Length == 0 || string_3.Length == 0)
            {
                result = "";
            }
            else
            {
                try
                {
                    string pattern = string.Concat(new string[]
                    {
                        "(",
                        GetUrlFormat4(string_2),
                        ")(.+?)(",
                        GetUrlFormat4(string_3),
                        ")"
                    });
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    foreach (object obj in regex.Matches(string_1))
                    {
                        Match match = (Match)obj;
                        arrayList.Add(match.Value);
                    }
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        if (i == 0)
                        {
                            text += arrayList[i].ToString();
                        }
                        else
                        {
                            text = text + "<|||>" + arrayList[i].ToString();
                        }
                    }
                    text = text.Replace(string_2, "");
                    text = text.Replace(string_3, "");
                    result = text;
                }
                catch
                {
                    result = "";
                }
            }
            return result;
        }

        /// <summary>
        /// url格式处理3
        /// </summary>
        /// <param name="string_1"></param>
        /// <param name="string_2"></param>
        /// <returns></returns>
        private string[] GetUrlFormat3(string string_1, string string_2)
        {
            return Regex.Split(string_1, GetUrlFormat4(string_2));
        }

        public static string GetUrlFormat4(string string_1)
        {
            string_1 = string_1.Replace("\\", "\\\\");
            string_1 = string_1.Replace("~", "\\~");
            string_1 = string_1.Replace("!", "\\!");
            string_1 = string_1.Replace("@", "\\@");
            string_1 = string_1.Replace("#", "\\#");
            string_1 = string_1.Replace("%", "\\%");
            string_1 = string_1.Replace("^", "\\^");
            string_1 = string_1.Replace("&", "\\&");
            string_1 = string_1.Replace("*", "\\*");
            string_1 = string_1.Replace("(", "\\(");
            string_1 = string_1.Replace(")", "\\)");
            string_1 = string_1.Replace("-", "\\-");
            string_1 = string_1.Replace("+", "\\+");
            string_1 = string_1.Replace("[", "\\[");
            string_1 = string_1.Replace("]", "\\]");
            string_1 = string_1.Replace("<", "\\<");
            string_1 = string_1.Replace(">", "\\>");
            string_1 = string_1.Replace(".", "\\.");
            string_1 = string_1.Replace("/", "\\/");
            string_1 = string_1.Replace("?", "\\?");
            string_1 = string_1.Replace("=", "\\=");
            string_1 = string_1.Replace("|", "\\|");
            string_1 = string_1.Replace("$", "\\$");
            return string_1;
        }

        #endregion
    }
}
