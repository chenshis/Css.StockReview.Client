using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using StockReview.Client.ContentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ViewModels
{
    public class LeadingGroupPromotionViewModel : BindableBase
    {
        public LeadingGroupPromotionViewModel()
        {
            InitTableHeader();
        }


        /// <summary>
        /// 初始化表头
        /// </summary>
        private void InitTableHeader()
        {
            var dateTime = DateTime.Now;

            string[] days = { "周一", "周二", "周三", "周四", "周五" };

            int workDayToAdd = 20;
            int workDaysCount = 0;

            while (workDaysCount < workDayToAdd)
            {
                int month = dateTime.Month;
                int day = dateTime.Day;
                DayOfWeek dayOfWeek = dateTime.DayOfWeek;

                if (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday)
                {
                    var workDayInfo = string.Format("{0}\n{1}\n{2}", month + "月" + day + "日", days[(int)dayOfWeek], 1);
                    workDaysCount++;
                }

                dateTime = dateTime.AddDays(1);
            }

        }
    }
}
