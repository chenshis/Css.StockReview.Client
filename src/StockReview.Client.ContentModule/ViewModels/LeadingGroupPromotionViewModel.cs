using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class LeadingGroupPromotionViewModel : NavigationAwareViewModelBase
    {
        public ObservableCollection<LeadingDateHeaderList> LeadingDateHeaderLists { get; set; } = new ObservableCollection<LeadingDateHeaderList>();

        public LeadingGroupPromotionViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this.PageTitle = "龙头晋级";
            InitTableHeader(); //组织头部
        }



            /// <summary>
            /// 初始化表头
            /// </summary>
        private void InitTableHeader()
        {
            var dateTime = DateTime.Now;

            string[] days = { "周日","周一", "周二", "周三", "周四", "周五","周六" };

            int workDayToAdd = 20;
            int workDaysCount = 1;

            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 0,
                HeadColumn = 0,
                HeadName = "版数"
            });

            while (workDaysCount <= workDayToAdd)
            {
                int month = dateTime.Month;
                int day = dateTime.Day;
                DayOfWeek dayOfWeek = dateTime.DayOfWeek;

                if (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday)
                {
                    LeadingDateHeaderLists.Add(new LeadingDateHeaderList
                    {
                        HeadRow=0,
                        HeadColumn = workDaysCount,
                        HeadName= string.Format("{0}\n{1}\n{2}", month + "月" + day + "日", days[(int)dayOfWeek], 1)
                    }) ;

                    LeadingDateHeaderLists.Add(new LeadingDateHeaderList
                    {
                        HeadRow = 1,
                        HeadColumn = workDaysCount,
                        HeadName = string.Format("{0}\n{1}\n{2}", "轻型输送带", "外销", "年报预增"),
                        
                    });
                    workDaysCount++;
                }

                dateTime = dateTime.AddDays(1);
            }

            #region 左侧菜单
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 3,
                HeadColumn = 0,
                HeadName = string.Format("13板太"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 4,
                HeadColumn = 0,
                HeadName = string.Format("12板月"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 5,
                HeadColumn = 0,
                HeadName = string.Format("11板量"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 6,
                HeadColumn = 0,
                HeadName = string.Format("10板天"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 7,
                HeadColumn = 0,
                HeadName = string.Format("9板飞"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 8,
                HeadColumn = 0,
                HeadName = string.Format("8板仙"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 9,
                HeadColumn = 0,
                HeadName = string.Format("7板神"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 10,
                HeadColumn = 0,
                HeadName = string.Format("6板鬼"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 11,
                HeadColumn = 0,
                HeadName = string.Format("5板魔"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 12,
                HeadColumn = 0,
                HeadName = string.Format("4板怪"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 13,
                HeadColumn = 0,
                HeadName = string.Format("3板妖"),
            });
            LeadingDateHeaderLists.Add(new LeadingDateHeaderList
            {
                HeadRow = 14,
                HeadColumn = 0,
                HeadName = string.Format("2板龙"),
            });
            #endregion

            for (int i = 0; i < 10; i++) 
            {
                LeadingDateHeaderLists.Add(new LeadingDateHeaderList
                {
                    HeadRow = 3,
                    HeadColumn = 1,
                    HeadName = string.Format("北京瑞华"),
                });
            }

            for (int i = 0; i < 5; i++)
            {
                LeadingDateHeaderLists.Add(new LeadingDateHeaderList
                {
                    HeadRow = 3,
                    HeadColumn = 4,
                    HeadName = string.Format("北京瑞华"),
                });
            }

            for (int i = 0; i < 5; i++)
            {
                LeadingDateHeaderLists.Add(new LeadingDateHeaderList
                {
                    HeadRow = 8,
                    HeadColumn = 3,
                    HeadName = string.Format("北京瑞华"),
                });
            }


            for (int i = 0; i < 10; i++)
            {
                LeadingDateHeaderLists.Add(new LeadingDateHeaderList
                {
                    HeadRow = 5,
                    HeadColumn = 6,
                    HeadName = string.Format("北京瑞华"),
                });
            }

            for (int i = 0; i < 5; i++)
            {
                LeadingDateHeaderLists.Add(new LeadingDateHeaderList
                {
                    HeadRow = 5,
                    HeadColumn = 8,
                    HeadName = string.Format("北京瑞华"),
                });
            }

            for (int i = 0; i < 5; i++)
            {
                LeadingDateHeaderLists.Add(new LeadingDateHeaderList
                {
                    HeadRow = 5,
                    HeadColumn = 10,
                    HeadName = string.Format("北京瑞华"),
                });
            }
        }
    }

  

    public class LeadingDateHeaderList
    {
        /// <summary>
        /// 行
        /// </summary>
        public int HeadRow { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int HeadColumn { get; set; }

        /// <summary>
        /// 头部名称
        /// </summary>
        public string HeadName { get; set; }

    }


}
