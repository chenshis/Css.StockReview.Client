using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using StockReview.Infrastructure.Config;
using StockReview.Api.Dtos;

namespace StockReview.Server.BackgroundServices
{
    public class BulletinBoardBackgroundService : StockReviewBackgroundService
    {
        public BulletinBoardBackgroundService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Schedule = "0/1 * * * * *";
        }

        protected override string Schedule { get; set; }

        protected override Task Process(IServiceProvider serviceProvider)
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            var httpClientToday = httpClientFactory.CreateClient(SystemConstant.TodayLonghuVipUrl);
            var httpClientHis = httpClientFactory.CreateClient(SystemConstant.HistoryLonghuVipUrl);

            BulletinBoardDto bulletinBoard = new();
            try
            {
                return Task.CompletedTask;
                var yesterday = DateTime.Now.AddDays(-1).ToShortDateString();
                var formUrlContent = GetFormUrlEncodedContent("ZhangFuDetail");
                var responseMessage = httpClientToday.PostAsync(default(string), formUrlContent).Result;
                var zhangFuDetailStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                JObject jobject = (JObject)JsonConvert.DeserializeObject(zhangFuDetailStrResult);
                JObject jobjectInfo = (JObject)jobject["info"];
                bulletinBoard.TodayLimitUp = jobjectInfo["SJZT"]?.ToString() ?? "--";
                bulletinBoard.TodayLimitDown = jobjectInfo["SJDT"]?.ToString() ?? "--";
                bulletinBoard.TodayRise = jobjectInfo["SZJS"]?.ToString() ?? "--";
                bulletinBoard.TodayFall = jobjectInfo["XDJS"]?.ToString() ?? "--";
                bulletinBoard.TodayCalorimeter = Math.Round(Convert.ToDouble(jobjectInfo["qscln"].ToString()) / 10000.0, 0).ToString() + "亿";


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


                formUrlContent = GetFormUrlEncodedContent("ZhangTingExpression");
                responseMessage = httpClientToday.PostAsync(default(string), formUrlContent).Result;
                var zhangTingExpStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                jobject = (JObject)JsonConvert.DeserializeObject(zhangTingExpStrResult);
                jobjectInfo = (JObject)jobject["info"];
                bulletinBoard.SecondBoardPercent = Math.Floor(Convert.ToDouble(jobjectInfo[4].ToString())).ToString() + "%";
                bulletinBoard.ThirdBoardPercent = Math.Round(Convert.ToDouble(jobjectInfo[5].ToString())).ToString() + "%";
                bulletinBoard.HighBoardPercent = Math.Round(Convert.ToDouble(jobjectInfo[6].ToString())).ToString() + "%";
                bulletinBoard.TodayFryingRate = Math.Round(Convert.ToDouble(jobjectInfo[7].ToString())).ToString() + "%";


                formUrlContent = GetFormUrlEncodedContent("DiskReview");
                responseMessage = httpClientToday.PostAsync(default(string), formUrlContent).Result;
                var diskReviewStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                jobject = (JObject)JsonConvert.DeserializeObject(diskReviewStrResult);
                jobjectInfo = (JObject)jobject["info"];
                bulletinBoard.EmotionPercent = jobjectInfo["strong"].ToString();


                formUrlContent = GetFormUrlEncodedContent(a: "ZhangTingExpression", c: "HisHomeDingPan", apiv: SystemConstant.apivW36, verSion: SystemConstant.VerSion51404, Day: yesterday);
                responseMessage = httpClientHis.PostAsync(default(string), formUrlContent).Result;
                var hisZhangTingExpStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                jobject = (JObject)JsonConvert.DeserializeObject(hisZhangTingExpStrResult);
                jobjectInfo = (JObject)jobject["info"];
                bulletinBoard.YesterdayFryingRate = Math.Round(Convert.ToDouble(jobjectInfo[7].ToString())).ToString() + "%";


                formUrlContent = GetFormUrlEncodedContent("MarketCapacity");
                responseMessage = httpClientToday.PostAsync(default(string), formUrlContent).Result;
                var marketCapacityStrResult = responseMessage.Content.ReadAsStringAsync().Result;
                jobject = (JObject)JsonConvert.DeserializeObject(marketCapacityStrResult);
                jobjectInfo = (JObject)jobject["info"];
                bulletinBoard.CityPower = jobjectInfo["yclnstr"].ToString();

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }



            return Task.CompletedTask;
        }

        private FormUrlEncodedContent GetFormUrlEncodedContent(string a,
                                                             string c = SystemConstant.HomeDingPan,
                                                             string apiv = SystemConstant.apivW31,
                                                             string phoneOSNew = SystemConstant.PhoneOSNew,
                                                             string deviceID = SystemConstant.DeviceID,
                                                             string verSion = SystemConstant.VerSion57012,
                                                             string Day = null)
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

            var content = new FormUrlEncodedContent(parameters);
            return content;
        }


    }
}
