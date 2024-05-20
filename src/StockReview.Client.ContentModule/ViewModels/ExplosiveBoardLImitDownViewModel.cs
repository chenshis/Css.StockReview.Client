using Prism.Regions;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using System.Collections.ObjectModel;
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

        public ExplosiveBoardLImitDownViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IReplayService replayService) : base(unityContainer, regionManager)
        {
            this.PageTitle = "炸板与跌停板";
            this._replayService = replayService;
            InitTableHeader();
        }

        private void InitTableHeader()
        {
            var explosiveList = this._replayService.GetExplosiveBoardLImitDown(DateTime.Now);

            ExplosiveFriedIndividualInfos.AddRange(explosiveList.ExplosiveFriedIndividualInfos);
            ExplosiveLimitUpStaticsInfos.AddRange(explosiveList.ExplosiveLimitUpStaticsInfos);
            ExplosiveLimitDownStaticsInfos.AddRange(explosiveList.ExplosiveLimitDownStaticsInfos);
            ExplosiveYeasterdayLimitUpStaticsInfos.AddRange(explosiveList.ExplosiveYeasterdayLimitUpStaticsInfos);
        }
    }
 
}
