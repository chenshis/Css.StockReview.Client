using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class ExplosiveBoardLImitDownViewModel : NavigationAwareViewModelBase
    {
        public ObservableCollection<ExplosiveFriedIndividualInfo> ExplosiveFriedIndividualInfos { get; set; } = new ObservableCollection<ExplosiveFriedIndividualInfo>();
        public ObservableCollection<ExplosiveLimitUpStaticsInfo> ExplosiveLimitUpStaticsInfos { get; set; } = new ObservableCollection<ExplosiveLimitUpStaticsInfo>();
        public ObservableCollection<ExplosiveLimitDownStaticsInfo> ExplosiveLimitDownStaticsInfos { get; set; } = new ObservableCollection<ExplosiveLimitDownStaticsInfo>();
        public ObservableCollection<ExplosiveYeasterdayLimitUpStaticsInfo> ExplosiveYeasterdayLimitUpStaticsInfos { get; set; } = new ObservableCollection<ExplosiveYeasterdayLimitUpStaticsInfo>();

        public ExplosiveBoardLImitDownViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this.PageTitle = "炸板与跌停板";
            InitTableHeader();
        }

        private void InitTableHeader()
        {
            for (int i = 0; i < 10; i++)
            {
                ExplosiveFriedIndividualInfos.Add(new ExplosiveFriedIndividualInfo
                {
                    Number = i.ToString(),
                    ExpSharesName = "瑞华",
                    ExpSharesCirculatingMarketValue = "100",
                    ExpSharesConcept = "鸿蒙",
                    ExpSharesFirstSealingTime = DateTime.Now.ToString("HH:mm:ss"),
                    ExpSharesLastFryingTime = DateTime.Now.ToString("HH:mm:ss"),
                    ExpSharesStartConnectedBoard = "10",
                    ExpSharesStartDoingBusiness = "10",
                    ExpSharesTailSealingTime = DateTime.Now.ToString("HH:mm:ss"),
                });
            }

            for (int i = 0; i < 5; i++)
            {
                ExplosiveLimitUpStaticsInfos.Add(new ExplosiveLimitUpStaticsInfo
                {
                    Number = i.ToString(),
                    ExpLimitName = i + "系统",
                    ExpLimitTotal = "1",
                    ExpLimitDown = "1",
                    ExpLimitUp = "1",
                });
            }

            for (int i = 0; i < 5; i++)
            {
                ExplosiveLimitDownStaticsInfos.Add(new ExplosiveLimitDownStaticsInfo
                {
                    Number = i.ToString(),
                    ExpDownName = "瑞华分公司" + i,
                    ExpDownFirstSealingTime = DateTime.Now.ToString("HH:mm:ss"),
                    ExpDownTailSealingTime = DateTime.Now.ToString("HH:mm:ss"),
                    ExpDownStartConnectedBoard = "10",
                    ExpDownStartDoingBusiness = "10"
                });
            }

            for (int i = 0; i < 5; i++)
            {
                ExplosiveYeasterdayLimitUpStaticsInfos.Add(new ExplosiveYeasterdayLimitUpStaticsInfo
                {
                    Number = i.ToString(),
                    ExpYeaName = "瑞华分公司" + i,
                    ExpYeaChange = "1",
                    ExpYeaConsecutiveBoard = "1",
                    ExpYeaIncrease = "1",
                    ExpYeaModule = "1"
                });
            }
        }
    }
    public class ExplosiveYeasterdayLimitUpStaticsInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string ExpYeaName { get; set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public string ExpYeaIncrease { get; set; }
        /// <summary>
        /// 昨连板
        /// </summary>
        public string ExpYeaConsecutiveBoard { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string ExpYeaModule { get; set; }
        /// <summary>
        /// 变化
        /// </summary>
        public string ExpYeaChange { get; set; }
    }

    public class ExplosiveLimitDownStaticsInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string ExpDownName { get; set; }

        /// <summary>
        /// 首封时间
        /// </summary>
        public string ExpDownFirstSealingTime { get; set; }

        /// <summary>
        /// 尾封时间
        /// </summary>
        public string ExpDownTailSealingTime { get; set; }

        /// <summary>
        /// 开板
        /// </summary>
        public string ExpDownStartDoingBusiness { get; set; }

        /// <summary>
        /// 连板
        /// </summary>
        public string ExpDownStartConnectedBoard { get; set; }
    }

    public class ExplosiveLimitUpStaticsInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 板块名称
        /// </summary>
        public string ExpLimitName { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public string ExpLimitTotal { get; set; }
        /// <summary>
        /// 上涨
        /// </summary>
        public string ExpLimitUp { get; set; }
        /// <summary>
        /// 下跌
        /// </summary>
        public string ExpLimitDown { get; set; }

    }

    public class ExplosiveFriedIndividualInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string ExpSharesName { get; set; }

        /// <summary>
        /// 板块概念
        /// </summary>
        public string ExpSharesConcept { get; set; }

        /// <summary>
        /// 首封时间
        /// </summary>
        public string ExpSharesFirstSealingTime { get; set; }

        /// <summary>
        /// 尾封时间
        /// </summary>
        public string ExpSharesTailSealingTime { get; set; }

        /// <summary>
        /// 最后炸板时间
        /// </summary>
        public string ExpSharesLastFryingTime { get; set; }

        /// <summary>
        /// 开板
        /// </summary>
        public string ExpSharesStartDoingBusiness { get; set; }

        /// <summary>
        /// 连板
        /// </summary>
        public string ExpSharesStartConnectedBoard { get; set; }

        /// <summary>
        /// 流通市值
        /// </summary>
        public string ExpSharesCirculatingMarketValue { get; set; }
    }
}
