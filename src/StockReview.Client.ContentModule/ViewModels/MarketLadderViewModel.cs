using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class MarketLadderViewModel : NavigationAwareViewModelBase
    {
        public string MarketTitle { get; set; }

        public ObservableCollection<MarketLadderNewsList> MarketLadderNewsLists { get; set; } = new ObservableCollection<MarketLadderNewsList>();
        public ObservableCollection<MarketLadderList> MarketLadderLists { get; set; } = new ObservableCollection<MarketLadderList>();

        public MarketLadderViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this.PageTitle = "市场天梯";
            this.MarketTitle = " 20240506 涨停25只 晋级率：5.26% 炸板率：50.98% 竞价涨幅：0.0696%";
            InitMarketLadderNewsDate();
        }

        private void InitMarketLadderNewsDate()
        {
            for (int i = 0; i < 20; i++)
            {
                MarketLadderNewsLists.Add(new MarketLadderNewsList
                {
                    MarketNewsType = 1,
                    MarketNewsTitle = "五连板",
                    MarketNewsTitleFontColor = "#F06632"
                });

                for (int j = 0; j < 3; j++)
                {
                    MarketLadderNewsLists.Add(new MarketLadderNewsList
                    {
                        MarketNewsType = 2,
                        MarketNewsTitle = "20240506 涨停25只",
                        MarketNewsTitleFontColor = "#EEE"
                    });
                }
            }

            for (int i = 0; i < 20; i++)
            {
                var MarketLadderToLists = new MarketLadderList() { MarketLadderInfos = new ObservableCollection<MarketLadderInfo>() };
                var MarketLadderInfos = new List<MarketLadderInfo>();
                MarketLadderToLists.MarketLadderBoard = i + "连板";
                MarketLadderToLists.MarketLadderNumber = i + "只";
                MarketLadderToLists.MarketLadderDescibe = string.Format("(晋级率：{0}%   炸板率：{1}%   竞价涨幅：{2}% )", i, i, i);

                for (int j = 0; j < 10; j++)
                {
                    MarketLadderInfos.Add(new MarketLadderInfo
                    {
                        MarketLadderCode = j.ToString(),
                        MarketLadderFirstLimitUp = DateTime.Now.ToString("HH:mm:ss"),
                        MarketLadderName = j.ToString(),
                        MarketLadderReasonLimitUp = j.ToString(),
                    });
                }
                MarketLadderToLists.MarketLadderInfos.AddRange(MarketLadderInfos);
                MarketLadderLists.Add(MarketLadderToLists);
            }
        }
    }

    public class MarketLadderList
    {
        /// <summary>
        /// 连板
        /// </summary>
        public string MarketLadderBoard { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string MarketLadderNumber { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string MarketLadderDescibe { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public ObservableCollection<MarketLadderInfo> MarketLadderInfos { get; set; }
    }

    public class MarketLadderInfo 
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string MarketLadderCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string MarketLadderName { get; set; }

        /// <summary>
        /// 首次涨停
        /// </summary>
        public string MarketLadderFirstLimitUp { get; set; }

        /// <summary>
        /// 涨停原因
        /// </summary>
        public string MarketLadderReasonLimitUp { get; set; }
    }

    public class MarketLadderNewsList
    {
        /// <summary>
        /// 新闻名称
        /// </summary>
        public string MarketNewsTitle { get; set; }

        /// <summary>
        /// 新闻类型
        /// </summary>
        public int MarketNewsType { get; set; }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public string MarketNewsTitleFontColor { get; set; }
    }
}
