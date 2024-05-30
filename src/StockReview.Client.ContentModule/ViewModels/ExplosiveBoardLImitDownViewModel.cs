using Prism.Commands;
using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
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

        public ExplosiveBoardLImitDownViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService) : base(unityContainer, regionManager)
        {
            this.PageTitle = "炸板与跌停板";
            this._replayService = replayService;
            CurrentDate = DateTime.Parse(DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd"));
            this.CurrentDate = CurrentDate;
            InitTableHeader(this.CurrentDate ?? DateTime.Now.AddDays(-1)); //组织头部
        }

        private void InitTableHeader(DateTime date)
        {
            var explosiveList = this._replayService.GetExplosiveBoardLImitDown(date);

            ExplosiveFriedIndividualInfos.Clear();
            ExplosiveLimitUpStaticsInfos.Clear();
            ExplosiveLimitDownStaticsInfos.Clear();
            ExplosiveYeasterdayLimitUpStaticsInfos.Clear();

            ExplosiveFriedIndividualInfos.AddRange(explosiveList.ExplosiveFriedIndividualInfos);
            ExplosiveLimitUpStaticsInfos.AddRange(explosiveList.ExplosiveLimitUpStaticsInfos);
            ExplosiveLimitDownStaticsInfos.AddRange(explosiveList.ExplosiveLimitDownStaticsInfos);
            ExplosiveYeasterdayLimitUpStaticsInfos.AddRange(explosiveList.ExplosiveYeasterdayLimitUpStaticsInfos);
        }

        private void Refresh()
        {
            InitTableHeader(this.CurrentDate ?? DateTime.Now);
        }
    }
 
}
