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

        private readonly IReplayService _replayService;

        public PlateRotationViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService) : base(unityContainer, regionManager)
        {
            this.PageTitle = "板块轮动";
            this._replayService = replayService;
            InitPlateRotation();
        }

        public void InitPlateRotation() 
        {
            var plateList = this._replayService.GetPlateRotation(DateTime.Now);

            this.PlateRotationHeaderTitle = plateList.PlateRotationHeaderTitle;

            this.PlateRotationInfosOne.AddRange(plateList.PlateRotationInfosOne);
            this.PlateRotationInfosTwo.AddRange(plateList.PlateRotationInfosTwo);
            this.PlateRotationInfosThree.AddRange(plateList.PlateRotationInfosThree);
            this.PlateRotationInfosFour.AddRange(plateList.PlateRotationInfosFour);
            this.PlateRotationInfosFive.AddRange(plateList.PlateRotationInfosFive);
            this.PlateRotationInfosSix.AddRange(plateList.PlateRotationInfosSix);

            this.PlateSharesLimitUpInfosOne.AddRange(plateList.PlateSharesLimitUpInfosOne);
            this.PlateSharesLimitUpInfosTwo.AddRange(plateList.PlateSharesLimitUpInfosTwo);
            this.PlateSharesLimitUpInfosThree.AddRange(plateList.PlateSharesLimitUpInfosThree);
            this.PlateSharesLimitUpInfosFour.AddRange(plateList.PlateSharesLimitUpInfosFour);
            this.PlateSharesLimitUpInfosFive.AddRange(plateList.PlateSharesLimitUpInfosFive);
            this.PlateSharesLimitUpInfosSix.AddRange(plateList.PlateSharesLimitUpInfosSix);
        }
    }

}
