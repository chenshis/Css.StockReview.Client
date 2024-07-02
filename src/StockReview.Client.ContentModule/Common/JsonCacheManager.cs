using Newtonsoft.Json;
using StockReview.Client.ContentModule.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Client.ContentModule.Common
{
    public static class JsonCacheManager
    {
        private static readonly string MarkDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "markdata.json");

        public static MarkDataModel GetMarDataList()
        {
            if (File.Exists(MarkDataFolder))
            {
                var jsonString = File.ReadAllText(MarkDataFolder);
                return JsonConvert.DeserializeObject<MarkDataModel>(jsonString);
            }
            return new MarkDataModel();
        }

        public static void SetMarDataList(MarkDataModel data)
        {
            string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(MarkDataFolder, jsonString, Encoding.UTF8);
        }
    }
}
