using StockReview.Api.IApiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StockReview.Api.ApiService
{
    /// <summary>
    /// Stock Out Look Implementation
    /// </summary>
    public class StockOutlookServerApiService : IStockOutlookServerApiService
    {
        private readonly HttpClient _httpClient;

        public StockOutlookServerApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(StockOutlookServerApiService));
        }

        /// <summary>
        /// 今昨涨跌停
        /// </summary>
        public void TYLimitUpAndDown()
        {
            var url = "https://apphq.longhuvip.com/w1/api/index.php";
            var parameter = "?a=DiskReview&apiv=w31&c=HisHomeDingPan&PhoneOSNew=1&DeviceID=ffffffff-c95a-a223-ffff-ffffdcc9b654&VerSion=5.7.0.1&Day=2024-05-13";

            parameter = parameter.Replace("ffffffff-c95a-a223-ffff-ffffdcc9b654", string.Concat(new string[]
            {
                "ffffffff-",GetRandomValue(4),"-",GetRandomValue(4),"-ffff",GetRandomValue(8)
            }));
            var address = url + parameter;

                //= "Dalvik/2.1.0 (Linux; U; Android 5.1.1; V1938CT Build/LMY49I)";
            var responseMessage = _httpClient.PostAsync(address, null).Result;
            var result = responseMessage.Content.ReadAsStringAsync().Result;

            throw new NotImplementedException();
        }


        /// <summary>
        /// 生成随机值
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GetRandomValue(int length)
        {
            string defaultValue = "abcdefghijklmnopqrstuvwxyz0123456789";
            string returnValue = "";
            for (int i = 0; i < length; i++)
            {
                byte[] array = new byte[4];
                RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
                rngcryptoServiceProvider.GetBytes(array);
                var seed = BitConverter.ToInt32(array, 0);
                Random random = new Random(seed);
                var index = random.Next(defaultValue.Length);

                returnValue += defaultValue[index].ToString();
            }

            return returnValue;
        }
    }
}
