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
    public class PlateRotationViewModel : NavigationAwareViewModelBase
    {

        public PlateRotationHeaderTitle  PlateRotationHeaderTitle { get; set; }= new PlateRotationHeaderTitle();

        public ObservableCollection<PlateRotationInfo> PlateRotationInfosOne { get; set; } =new ObservableCollection<PlateRotationInfo>();
        public ObservableCollection<PlateRotationInfo> PlateRotationInfosTwo { get; set; } = new ObservableCollection<PlateRotationInfo>();
        public ObservableCollection<PlateRotationInfo> PlateRotationInfosThree { get; set; } = new ObservableCollection<PlateRotationInfo>();
        public ObservableCollection<PlateRotationInfo> PlateRotationInfosFour { get; set; } = new ObservableCollection<PlateRotationInfo>();
        public ObservableCollection<PlateRotationInfo> PlateRotationInfosFive { get; set; } = new ObservableCollection<PlateRotationInfo>();
        public ObservableCollection<PlateRotationInfo> PlateRotationInfosSix { get; set; } = new ObservableCollection<PlateRotationInfo>();

        public ObservableCollection<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosOne { get; set; } = new ObservableCollection<PlateSharesLimitUpInfo>();
        public ObservableCollection<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosTwo { get; set; } = new ObservableCollection<PlateSharesLimitUpInfo>();
        public ObservableCollection<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosThree { get; set; } = new ObservableCollection<PlateSharesLimitUpInfo>();
        public ObservableCollection<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosFour { get; set; } = new ObservableCollection<PlateSharesLimitUpInfo>();
        public ObservableCollection<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosFive { get; set; } = new ObservableCollection<PlateSharesLimitUpInfo>();
        public ObservableCollection<PlateSharesLimitUpInfo> PlateSharesLimitUpInfosSix { get; set; } = new ObservableCollection<PlateSharesLimitUpInfo>();

        public PlateRotationViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this.PageTitle = "板块轮动";
            InitPlateRotation();
        }



        public void InitPlateRotation() 
        {
            PlateRotationHeaderTitle.PlateDateOne = DateTime.Now.ToString("yyyy-MM-dd");
            PlateRotationHeaderTitle.PlateDateTwo = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            PlateRotationHeaderTitle.PlateDateThree =DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
            PlateRotationHeaderTitle.PlateDateFour = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd");
            PlateRotationHeaderTitle.PlateDateFive = DateTime.Now.AddDays(4).ToString("yyyy-MM-dd");
            PlateRotationHeaderTitle.PlateDateSix = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd");

            var plateRotationInfo = new List<PlateRotationInfo>();

            for (int i = 0; i < 10; i++) 
            {
                plateRotationInfo.Add(new PlateRotationInfo
                {
                    Number = i.ToString(),
                    PlateName = "煤矿",
                    PlateStrength = "100",
                    PlateMainBuy = "100",
                    PlateMainForce ="8000亿",
                });
            }

            PlateRotationInfosOne.AddRange(plateRotationInfo);
            PlateRotationInfosTwo.AddRange(plateRotationInfo);
            PlateRotationInfosThree.AddRange(plateRotationInfo);
            PlateRotationInfosFour.AddRange(plateRotationInfo);
            PlateRotationInfosFive.AddRange(plateRotationInfo);
            PlateRotationInfosSix.AddRange(plateRotationInfo);


            var plateSharesLimitUpInfos=new List<PlateSharesLimitUpInfo>();
            for (int i = 0; i < 10; i++)
            {
                plateSharesLimitUpInfos.Add(new PlateSharesLimitUpInfo
                {
                    Number = i.ToString(),
                    PlateSharesCode = "1001",
                    PlateSharesName = "北京瑞华",
                    PlateSharesPrice = "100",
                    PlateSharesIncrease = "100%",
                    PlateSharesStatus = "0",
                    PlateSharesMainForce = "中国华电",
                    PlateSharesTranVoume = "1000亿",
                    PlateSharesNumberBoards = "10",
                    PlateSharesCirculationValue = "100",
                });
            }
            PlateSharesLimitUpInfosOne.AddRange(plateSharesLimitUpInfos);
            PlateSharesLimitUpInfosTwo.AddRange(plateSharesLimitUpInfos);
            PlateSharesLimitUpInfosThree.AddRange(plateSharesLimitUpInfos);
            PlateSharesLimitUpInfosFour.AddRange(plateSharesLimitUpInfos);
            PlateSharesLimitUpInfosFive.AddRange(plateSharesLimitUpInfos);
            PlateSharesLimitUpInfosSix.AddRange(plateSharesLimitUpInfos);
        }
    }

    public class PlateSharesLimitUpInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string PlateSharesCode { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string PlateSharesName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string PlateSharesPrice { get; set; }

        /// <summary>
        /// 涨幅
        /// </summary>
        public string PlateSharesIncrease { get; set; }

        /// <summary>
        /// 地位
        /// </summary>
        public string PlateSharesStatus { get; set; }

        /// <summary>
        /// 连板数
        /// </summary>
        public string PlateSharesNumberBoards { get; set; }

        /// <summary>
        /// 主力
        /// </summary>
        public string PlateSharesMainForce { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public string PlateSharesTranVoume{ get; set; }

        /// <summary>
        /// 流通值
        /// </summary>
        public string PlateSharesCirculationValue { get; set; }
    }


    public class PlateRotationInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string PlateName { get; set; }

        /// <summary>
        /// 强度
        /// </summary>
        public string PlateStrength { get; set; }

        /// <summary>
        /// 主力净额
        /// </summary>
        public string PlateMainForce { get; set; }

        /// <summary>
        /// 主力买
        /// </summary>
        public string PlateMainBuy { get; set; }
    }


    public class PlateRotationHeaderTitle
    {
        /// <summary>
        /// 标题1-6
        /// </summary>
        public string PlateDateOne { get; set; }
        public string PlateDateTwo { get; set; }
        public string PlateDateThree { get; set; }
        public string PlateDateFour { get; set; }
        public string PlateDateFive { get; set; }
        public string PlateDateSix { get; set; }
    }
}
