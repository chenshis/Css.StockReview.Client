using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
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
        private readonly IReplayService _replayService;
        public DragonTigerViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService) : base(unityContainer, regionManager)
        {
            this.PageTitle = "龙虎榜";
            this._replayService = replayService;
            InitTableHeader(); //组织头部
        }

        private void InitTableHeader()
        {
            var dragonTigerList = this._replayService.GetDragonTiger(DateTime.Now);

            this.DragonTigerGetInfos.AddRange(dragonTigerList.DragonTigerGetInfos);
            this.DragonTigerGetInfosOne.AddRange(dragonTigerList.DragonTigerGetInfosOne);
            this.DragonTigerGetInfosTwo.AddRange(dragonTigerList.DragonTigerGetInfosTwo);
            this.DragonTigerGetInfosThree.AddRange(dragonTigerList.DragonTigerGetInfosThree);
            this.DragonTigerGetInfosFous.AddRange(dragonTigerList.DragonTigerGetInfosFous);
        }
    }
}
