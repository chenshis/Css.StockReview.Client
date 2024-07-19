using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Domain.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class PlateRotationViewModel : NavigationAwareViewModelBase
    {

        //public PlateRotationHeaderTitle PlateRotationHeaderTitle { get; set; }= new PlateRotationHeaderTitle();

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
        private readonly IEventAggregator _eventAggregator;
        #region 命令
        public ICommand RefreshCommand => new DelegateCommand<string>((k) =>
        {
            Refresh();
        });
      
        #endregion
        private DateTime? _currentDate;
        /// <summary>
        /// 选中日期
        /// </summary>
        public DateTime? CurrentDate
        {
            get { return _currentDate; }
            set { SetProperty(ref _currentDate, value); }
        }

        private string _plateDateOne;
     
        public string PlateDateOne
        {
            get { return _plateDateOne; }
            set { SetProperty(ref _plateDateOne, value); }
        }

        private string _plateDateTwo;

        public string PlateDateTwo
        {
            get { return _plateDateTwo; }
            set { SetProperty(ref _plateDateTwo, value); }
        }

        private string _plateDateThree;

        public string PlateDateThree
        {
            get { return _plateDateThree; }
            set { SetProperty(ref _plateDateThree, value); }
        }

        private string _plateDateFour;

        public string PlateDateFour
        {
            get { return _plateDateFour; }
            set { SetProperty(ref _plateDateFour, value); }
        }

        private string _plateDateFive;

        public string PlateDateFive
        {
            get { return _plateDateFive; }
            set { SetProperty(ref _plateDateFive, value); }
        }

        private string _plateDateSix;

        public string PlateDateSix
        {
            get { return _plateDateSix; }
            set { SetProperty(ref _plateDateSix, value); }
        }




        public PlateRotationViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService, IEventAggregator eventAggregator) : base(unityContainer, regionManager)
        {
            this.PageTitle = "板块轮动";
            this._eventAggregator = eventAggregator;
            this._replayService = replayService;
            CurrentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            this.CurrentDate = CurrentDate;
            _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
            InitTableHeader(this.CurrentDate ?? DateTime.Now); //组织头部
        }

        private void InitTableHeader(DateTime date)
        {
            Task.Run(() =>
            {
                var plateList = this._replayService.GetPlateRotation(date);

                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {

                    //this.PlateRotationHeaderTitle = new PlateRotationHeaderTitle();
                    this.PlateRotationInfosOne.Clear();
                    this.PlateRotationInfosTwo.Clear();
                    this.PlateRotationInfosThree.Clear();
                    this.PlateRotationInfosFour.Clear();
                    this.PlateRotationInfosFive.Clear();
                    this.PlateRotationInfosSix.Clear();

                    this.PlateSharesLimitUpInfosOne.Clear();
                    this.PlateSharesLimitUpInfosTwo.Clear();
                    this.PlateSharesLimitUpInfosThree.Clear();
                    this.PlateSharesLimitUpInfosFour.Clear();
                    this.PlateSharesLimitUpInfosFive.Clear();
                    this.PlateSharesLimitUpInfosSix.Clear();


                    this.PlateDateOne = plateList.PlateRotationHeaderTitle.PlateDateOne;
                    this.PlateDateTwo = plateList.PlateRotationHeaderTitle.PlateDateTwo;
                    this.PlateDateThree = plateList.PlateRotationHeaderTitle.PlateDateThree;
                    this.PlateDateFour = plateList.PlateRotationHeaderTitle.PlateDateFour;
                    this.PlateDateFive = plateList.PlateRotationHeaderTitle.PlateDateFive;
                    this.PlateDateSix = plateList.PlateRotationHeaderTitle.PlateDateSix;

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

                }));
                _eventAggregator.GetEvent<LoadingEvent>().Publish(false);
            });
          
        }
        private void Refresh()
        {
            _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
            InitTableHeader(this.CurrentDate ?? DateTime.Now);
        }
    }

}
