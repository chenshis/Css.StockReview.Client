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

            foreach (var item in plateList.PlateRotationInfosOne)
            {
                this.PlateRotationInfosOne.Add(item);
            }
            foreach (var item in plateList.PlateRotationInfosTwo)
            {
                this.PlateRotationInfosTwo.Add(item);
            }
            foreach (var item in plateList.PlateRotationInfosThree)
            {
                this.PlateRotationInfosThree.Add(item);
            }
            foreach (var item in plateList.PlateRotationInfosFour)
            {
                this.PlateRotationInfosFour.Add(item);
            }
            foreach (var item in plateList.PlateRotationInfosFive)
            {
                this.PlateRotationInfosFive.Add(item);
            }
            foreach (var item in plateList.PlateRotationInfosSix)
            {
                this.PlateRotationInfosSix.Add(item);
            }


            foreach (var item in plateList.PlateSharesLimitUpInfosOne)
            {
                this.PlateSharesLimitUpInfosOne.Add(item);
            }
            foreach (var item in plateList.PlateSharesLimitUpInfosTwo)
            {
                this.PlateSharesLimitUpInfosTwo.Add(item);
            }
            foreach (var item in plateList.PlateSharesLimitUpInfosThree)
            {
                this.PlateSharesLimitUpInfosThree.Add(item);
            }
            foreach (var item in plateList.PlateSharesLimitUpInfosFour)
            {
                this.PlateSharesLimitUpInfosFour.Add(item);
            }
            foreach (var item in plateList.PlateSharesLimitUpInfosFive)
            {
                this.PlateSharesLimitUpInfosFive.Add(item);
            }
            foreach (var item in plateList.PlateSharesLimitUpInfosSix)
            {
                this.PlateSharesLimitUpInfosSix.Add(item);
            }
        }
    }

}
