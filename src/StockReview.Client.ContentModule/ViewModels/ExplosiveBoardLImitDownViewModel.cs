using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Domain.Events;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class ExplosiveBoardLImitDownViewModel : NavigationAwareViewModelBase
    {
        public ObservableCollection<ExplosiveFriedIndividualInfo> ExplosiveFriedIndividualInfos { get; set; } = new ObservableCollection<ExplosiveFriedIndividualInfo>();
        public ObservableCollection<ExplosiveLimitUpStaticsInfo> ExplosiveLimitUpStaticsInfos { get; set; } = new ObservableCollection<ExplosiveLimitUpStaticsInfo>();
        public ObservableCollection<ExplosiveLimitDownStaticsInfo> ExplosiveLimitDownStaticsInfos { get; set; } = new ObservableCollection<ExplosiveLimitDownStaticsInfo>();
        public ObservableCollection<ExplosiveYeasterdayLimitUpStaticsInfo> ExplosiveYeasterdayLimitUpStaticsInfos { get; set; } = new ObservableCollection<ExplosiveYeasterdayLimitUpStaticsInfo>();

        private readonly IReplayService _replayService; 
        private readonly IEventAggregator _eventAggregator;
        private DateTime? _currentDate;
        /// <summary>
        /// 选中日期
        /// </summary>
        public DateTime? CurrentDate
        {
            get { return _currentDate; }
            set { SetProperty(ref _currentDate, value); }
        }


        #region 命令
        public ICommand RefreshCommand => new DelegateCommand<string>((k) =>
        {
            Refresh();
        });
       
        #endregion

        public ExplosiveBoardLImitDownViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService
            , IEventAggregator eventAggregator) : base(unityContainer, regionManager)
        {
            this.PageTitle = "炸板与跌停板";
            this._eventAggregator = eventAggregator;
            this._replayService = replayService;
            CurrentDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            this.CurrentDate = CurrentDate;
            _eventAggregator.GetEvent<LoadingEvent>().Publish(true);
            InitTableHeader(this.CurrentDate ?? DateTime.Now); //组织头部
        }

        private void InitTableHeader(DateTime date)
        {
            Task.Run(() =>
            {
                var explosiveList = this._replayService.GetExplosiveBoardLImitDown(date);

                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ExplosiveFriedIndividualInfos.Clear();
                    ExplosiveLimitUpStaticsInfos.Clear();
                    ExplosiveLimitDownStaticsInfos.Clear();
                    ExplosiveYeasterdayLimitUpStaticsInfos.Clear();

                    ExplosiveFriedIndividualInfos.AddRange(explosiveList.ExplosiveFriedIndividualInfos);
                    ExplosiveLimitUpStaticsInfos.AddRange(explosiveList.ExplosiveLimitUpStaticsInfos);
                    ExplosiveLimitDownStaticsInfos.AddRange(explosiveList.ExplosiveLimitDownStaticsInfos);
                    ExplosiveYeasterdayLimitUpStaticsInfos.AddRange(explosiveList.ExplosiveYeasterdayLimitUpStaticsInfos);
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
