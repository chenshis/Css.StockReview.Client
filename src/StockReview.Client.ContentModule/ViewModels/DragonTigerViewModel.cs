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
    public class DragonTigerViewModel : NavigationAwareViewModelBase
    {
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfos { get; set; }= new ObservableCollection<DragonTigerGetInfo>();

        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosOne { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosTwo { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosThree { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public ObservableCollection<DragonTigerGetInfo> DragonTigerGetInfosFous { get; set; } = new ObservableCollection<DragonTigerGetInfo>();
        public DragonTigerViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this.PageTitle = "龙虎榜";
            InitTableHeader(); //组织头部
        }

        private void InitTableHeader()
        {
            for (int i = 0; i < 5; i++) 
            {
                DragonTigerGetInfos.Add(new DragonTigerGetInfo
                {
                    Number = i.ToString(),
                    DragonName = i.ToString(),
                    DragonIncrease = i.ToString(),
                    DragonPlate = i.ToString(),
                    DragonPurchase = i.ToString(),
                    DragonSpeculative = i.ToString(),
                });
                DragonTigerGetInfosOne.Add(new DragonTigerGetInfo
                {
                    Number = i.ToString(),
                    DragonName = i.ToString(),
                    DragonIncrease = i.ToString(),
                    DragonPlate = i.ToString(),
                    DragonPurchase = i.ToString(),
                    DragonSpeculative = i.ToString(),
                });
                DragonTigerGetInfosTwo.Add(new DragonTigerGetInfo
                {
                    Number = i.ToString(),
                    DragonName = i.ToString(),
                    DragonIncrease = i.ToString(),
                    DragonPlate = i.ToString(),
                    DragonPurchase = i.ToString(),
                    DragonSpeculative = i.ToString(),
                });
                DragonTigerGetInfosThree.Add(new DragonTigerGetInfo
                {
                    Number = i.ToString(),
                    DragonName = i.ToString(),
                    DragonIncrease = i.ToString(),
                    DragonPlate = i.ToString(),
                    DragonPurchase = i.ToString(),
                    DragonSpeculative = i.ToString(),
                });
                DragonTigerGetInfosFous.Add(new DragonTigerGetInfo
                {
                    Number = i.ToString(),
                    DragonName = i.ToString(),
                    DragonIncrease = i.ToString(),
                    DragonPlate = i.ToString(),
                    DragonPurchase = i.ToString(),
                    DragonSpeculative = i.ToString(),
                });

            }
        }
    }


    public class DragonTigerGetInfo 
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string DragonName { get; set; }
        /// <summary>
        /// 游资
        /// </summary>
        public string DragonSpeculative { get; set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public string DragonIncrease { get; set; }
        /// <summary>
        /// 净买
        /// </summary>
        public string DragonPurchase { get; set; }
        /// <summary>
        /// 板
        /// </summary>
        public string DragonPlate { get; set; }
    }
}
